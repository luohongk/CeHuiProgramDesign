import utils
class CPoint:
  def __init__(self,name,x,y,z):
    self.name = name
    self.x = x
    self.y = y
    self.z = z
    self.i_in_grid = -1
    self.j_in_grid = -1

class CGrid:
  def __init__(self):
    self.point_cloud = []
    self.count = 0
    self.z_max = 0
    self.z_avg = 0
    self.z_diff = 0
    self.z_var = 0

  def add_point(self,point):
    self.point_cloud.append(point)
    self.count+=1

  def update_statu(self):
    z_value = [p.z for p in self.point_cloud]
    self.z_avg = sum(z_value) / self.count
    self.z_max = max(z_value)
    self.z_diff = max(z_value) - min(z_value)
    self.z_var = sum((z-self.z_avg)**2 for z in z_value) / self.count

  def output_info(self):
    print(f'point_num:{self.count}')

class planeError(Exception):
  pass
class CPlane:
  def __init__(self,pA,pB,pC):
    self.A = self.B = self.C = self.D = 0
    self.area = utils.get_S(pA,pB,pC)
    if self.area < 1e-3:
      raise planeError('三点共线')
    self.fitPlane(pA,pB,pC)
  def fitPlane(self,pA,pB,pC):
    AB_x = pB.x - pA.x
    AB_y = pB.y - pA.y
    AB_z = pB.z - pA.z
    AC_x = pC.x - pA.x
    AC_y = pC.y - pA.y
    AC_z = pC.z - pA.z
    self.A = AB_y * AC_z - AB_z * AC_y
    self.B = AB_z * AC_x - AB_x * AC_z
    self.C = AB_x * AC_y - AB_y * AC_x
    self.D = 0 - self.A * pA.x - self.B * pA.y - self.C * pA.z
  def compute_in_out(self,point_cloud):
    self.dis_point_plane = []
    self.inliers = []
    self.outliers = []
    self.inliers_num = 0
    self.outliers_num = 0
    for p in point_cloud:
      dis = utils.dis_point_plane(p,self)
      self.dis_point_plane.append(dis)
      if (dis < 0.1):
        self.inliers.append(p)
        self.inliers_num+=1
      else:
        self.outliers_num+=1
        self.outliers.append(p)
    self.inliers_num-=3