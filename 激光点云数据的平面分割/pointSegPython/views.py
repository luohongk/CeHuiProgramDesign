import tkinter as tk
def visualize_grids_and_points(point_cloud):
    pc_win = tk.Toplevel()
    canvas_width = 500
    canvas_height = 500
    canvas = tk.Canvas(pc_win, width=canvas_width, height=canvas_height, bg="white")
    canvas.pack()

    # 缩放和平移参数
    scale = min(canvas_width, canvas_height) // 100  # 每单位10像素
    offset_x = 10
    offset_y = canvas_height - 10

    # 绘制栅格线
    for i in range(11):  # 0~10
        x = i * 10 * scale
        y = i * 10 * scale
        # 横线
        canvas.create_line(offset_x, offset_y - y, offset_x + 100 * scale, offset_y - y, fill="lightgray")
        # 竖线
        canvas.create_line(offset_x + x, offset_y, offset_x + x, offset_y - 100 * scale, fill="lightgray")

    # 绘制点
    for pt in point_cloud:
        screen_x = offset_x + pt.x * scale
        screen_y = offset_y - pt.y * scale  # Y轴翻转
        canvas.create_oval(screen_x - 2, screen_y - 2, screen_x + 2, screen_y + 2,
                           fill="red", outline="red")
    pc_win.mainloop()