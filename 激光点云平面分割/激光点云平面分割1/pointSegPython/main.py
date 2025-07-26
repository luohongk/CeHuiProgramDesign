import tkinter as tk
from tkinter import filedialog
from myClass import CPoint,CGrid,CPlane
import views
import utils
from math import *

class app:
  def __init__(self,root):
    self.root = root
    self.create_widgets()
    self.point_cloud = []
    rows=cols=10
    self.grids = [[CGrid() for _ in range(cols)] for _ in range(rows)]
    self.best_plane = None
    self.second_best_plane = None

  def create_widgets(self):
    menu = tk.Menu(self.root)
    menu.add_command(label='打开文件',command=self.openfile)
    menu.add_command(label='分配栅格可视化',command=self.raster_point_cloud)
    menu.add_command(label='计算栅格单元信息',command=self.compute_grid_info)
    menu.add_command(label='拟合平面',command=self.fit_plane)
    menu.add_command(label='最佳平面分割',command=self.compute_seg_plane)
    menu.add_command(label='点云水平截面投影',command=self.project)
    menu.add_command(label='输出',command=self.output)
    #menu.add_command(label='平面拟合',command=fitplane)
    self.root.config(menu=menu)

    self.tbox = tk.Text(self.root)
    self.tbox.pack()

  def openfile(self):
    filepath = filedialog.askopenfilename()
    if filepath:
      with open(filepath,'r') as file:
        content = file.read()
        self.tbox.insert(tk.END,content)
      with open(filepath,'r') as file:
        num = int(file.readline())
        for i in range(num):
          line = file.readline()
          data = line.split(',')
          self.point_cloud.append(CPoint(data[0],
                                    float(data[1]),
                                    float(data[2]),
                                    float(data[3])))
  
  def raster_point_cloud(self):
    dx = 10
    dy = 10
    for point in self.point_cloud:
      i = floor(point.y / dy)
      j = floor(point.x / dx)
      point.i_in_grid = i
      point.j_in_grid = j
      self.grids[i][j].add_point(point)
    views.visualize_grids_and_points(self.point_cloud)
  
  def compute_grid_info(self):
    input_ij = tk.Toplevel()
    input_ij.title('输入行列号')

    tk.Label(input_ij,text='行号(i):').grid(row=0,column=0)
    entry_i = tk.Entry(input_ij)
    entry_i.grid(row=0,column=1)
    tk.Label(input_ij,text='列号(j):').grid(row=1,column=0)
    entry_j = tk.Entry(input_ij)
    entry_j.grid(row=1,column=1)

    def on_submit():
      i = int(entry_i.get())
      j = int(entry_j.get())
      self.grids[i][j].update_statu()
      #grids[i][j].output_info()
      z_avg = self.grids[i][j].z_avg
      z_max = self.grids[i][j].z_max
      z_diff = self.grids[i][j].z_diff
      z_var = self.grids[i][j].z_var
      result = f'grid({i},{j}):\n z_avg:{z_avg}\n z_max:{z_max}\n z_diff:{z_diff}\n z_var:{z_var}\n'
      self.tbox.delete(1.0,tk.END)
      self.tbox.insert(tk.END,result)
      input_ij.destroy()

    buttonOK = tk.Button(input_ij,text='确定',command=on_submit)
    buttonOK.grid(row=2,column=0)

  def fit_plane(self):
    fit_win = tk.Toplevel()
    tip = tk.Label(fit_win,text='三个点的index(例如P1的index为0,P2的index为1):')
    tip.pack()
    pa_entry = tk.Entry(fit_win)
    pb_entry = tk.Entry(fit_win)
    pc_entry = tk.Entry(fit_win)
    pa_entry.pack()
    pb_entry.pack()
    pc_entry.pack()
    def on_submit():
      pa_index = int(pa_entry.get())
      pb_index = int(pb_entry.get())
      pc_index = int(pc_entry.get())
      plane = CPlane(self.point_cloud[pa_index],self.point_cloud[pb_index],self.point_cloud[pc_index])
      result = f'A = {plane.A}\nB = {plane.B}\nC = {plane.C}\nD = {plane.D}\narea = {plane.area}\n'
      self.tbox.delete(1.0,tk.END)
      self.tbox.insert(tk.END,result)
      
    buttonOK = tk.Button(fit_win,text='确认',command=on_submit)
    buttonOK.pack()

  def compute_seg_plane(self):
    planes = []
    max_inlier_num = 0
    for i in range(300):
      pA = self.point_cloud[3*i+0]
      pB = self.point_cloud[3*i+1]
      pC = self.point_cloud[3*i+2]
      try:
        plane = CPlane(pA,pB,pC)
        plane.compute_in_out(point_cloud=self.point_cloud)
        planes.append(plane)
        if (plane.inliers_num > max_inlier_num):
          max_inlier_num = plane.inliers_num
          self.best_plane = plane
      except:
        planes.append(None)
        pass
    self.second_point_cloud = self.best_plane.outliers
    max_inlier_num = 0
    for i in range(80):
      pA = self.second_point_cloud[3*i+0]
      pB = self.second_point_cloud[3*i+1]
      pC = self.second_point_cloud[3*i+2]
      try:
        plane = CPlane(pA,pB,pC)
        plane.compute_in_out(point_cloud=self.second_point_cloud)
        planes.append(plane)
        if (plane.inliers_num > max_inlier_num):
          max_inlier_num = plane.inliers_num
          self.second_best_plane = plane
      except:
        planes.append(None)
        pass
    #在tbox中输出22,23,24,25
    plane1 = planes[0]
    dis1000 = plane1.dis_point_plane[999]
    dis5 = plane1.dis_point_plane[4]
    num_in = plane1.inliers_num
    num_out = plane1.outliers_num
    result = f'P1000到拟合平面S1的距离 :{dis1000}\nP5到拟合平面S1的距离 :{dis5}\n拟合平面S1的内部点数量 :{num_in}\n拟合平面S1的外部点数量: {num_out}\n'
    self.tbox.delete(1.0,tk.END)
    self.tbox.insert(tk.END,result)
    #在tbox中输出最佳分割平面
    result_best = f'最佳分割平面\nA:{self.best_plane.A}\nB:{self.best_plane.B}\nC:{self.best_plane.C}\nD:{self.best_plane.D}\ninliers:{self.best_plane.inliers_num}\noutliers:{self.best_plane.outliers_num}\n'
    self.tbox.insert(tk.END,result_best)
    #在tbox中输出第二分割平面
    result_best = f'第二分割平面\nA:{self.second_best_plane.A}\nB:{self.second_best_plane.B}\nC:{self.second_best_plane.C}\nD:{self.second_best_plane.D}\ninliers:{self.second_best_plane.inliers_num}\noutliers:{self.second_best_plane.outliers_num}\n'
    self.tbox.insert(tk.END,result_best)

  def project(self):
    input_win = tk.Toplevel()
    tk.Label(input_win,text='输入点索引(P1索引为0,P2索引为1):').grid(row=0,column=0)
    pt_entry = tk.Entry(input_win)
    pt_entry.grid(row=0,column=1)
    tk.Label(input_win,text='输入投影平面号(最佳平面J1为1,第二平面J2为2):').grid(row=1,column=0)
    plane_entry = tk.Entry(input_win)
    plane_entry.grid(row=1,column=1)
    def on_submit():
      pt_index = int(pt_entry.get())
      plane_index = int(plane_entry.get())
      point = self.point_cloud[pt_index]
      plane = self.best_plane
      if (plane_index==2):
        plane = self.second_best_plane
      xt,yt,zt = utils.point_pro2plane(point,plane)
      result = f'(xt,yt,zt) = ({xt},{yt},{zt})'
      self.tbox.delete(1.0,tk.END)
      self.tbox.insert(tk.END,result)
    buttonOK = tk.Button(input_win,text='确定',command=on_submit)
    buttonOK.grid(row=2,column=0)

  def output(self):
    output_path = './result.txt'
    p_plane = [0 for _ in range(1000)]
    for p in self.best_plane.inliers:
      p_index = int(p.name[1:])-1
      p_plane[p_index] = 1
    for p in self.second_best_plane.inliers:
      p_index = int(p.name[1:])-1
      p_plane[p_index] = 2
    with open(output_path,'w') as file:
      for i in range(1000):
        if (p_plane[i]==0):
          line=f'P{i+1},{self.point_cloud[i].x},{self.point_cloud[i].y},{self.point_cloud[i].z},0\n'
        elif(p_plane[i]==1):
          line=f'P{i+1},{self.point_cloud[i].x},{self.point_cloud[i].y},{self.point_cloud[i].z},J1\n'
        else:
          line=f'P{i+1},{self.point_cloud[i].x},{self.point_cloud[i].y},{self.point_cloud[i].z},J2\n'
        file.write(line)
      
def main():
  root = tk.Tk()
  app(root)
  root.mainloop()

if __name__=='__main__':
  main()