import math as m
def get_min_max(point_cloud):
  xmin = xmax = point_cloud[0].x
  ymin = ymax = point_cloud[0].y
  for point in point_cloud:
    if (point.x < xmin):
      xmin = point.x
    if (point.x > xmax):
      xmax = point.x
    if (point.y < ymin):
      ymin = point.y
    if (point.y > ymax):
      ymax = point.y
  return xmin,xmax,ymin,ymax

def dis(pA,pB):
  return m.sqrt((pA.x-pB.x)**2 + (pA.y - pB.y)**2 + (pA.z - pB.z)**2)

def get_S(pA,pB,pC):
  AB = dis(pA,pB)
  BC = dis(pB,pC)
  AC = dis(pA,pC)
  p = 0.5*(AB+BC+AC)
  return m.sqrt(p*(p-AB)*(p-BC)*(p-AC))

def dis_point_plane(p,plane):
  a = abs(plane.A * p.x + plane.B * p.y + plane.C * p.z + plane.D)
  b = m.sqrt(plane.A**2 + plane.B**2 + plane.C**2)
  return a / b

def point_pro2plane(p,plane):
  a = plane.A
  b = plane.B
  c = plane.C
  d = plane.D
  xt = ((b**2+c**2) * p.x - a*(b*p.y+c*p.z+d)) / (a**2 + b**2 + c**2)
  yt = ((a**2+c**2) * p.y - b*(a*p.x+c*p.z+d)) / (a**2 + b**2 + c**2)
  zt = ((a**2+b**2) * p.z - c*(a*p.x+b*p.y+d)) / (a**2 + b**2 + c**2)
  return xt,yt,zt