#!/usr/bin/env python
# -*- coding: utf-8 -*-

import math
import pandas as pd
import os

def row_to_letter(row):
    return chr(ord('A') + row - 1)

def calc_100w_grid(longitude, latitude):
    row = int(latitude / 4) + 1
    col = int(longitude / 6) + 31
    
    row_letter = row_to_letter(row)
    
    return f"{row_letter}{col}"

def calc_diff(longitude, latitude):
    lon_diff = longitude - int(longitude / 6) * 6
    lat_diff = latitude - int(latitude / 4) * 4
    return lon_diff, lat_diff

def calc_grid_row_col(longitude, latitude, lon_diff_scale, lat_diff_scale):
    row = int(4 / lat_diff_scale - (latitude % 4) / lat_diff_scale) + 1
    col = int((longitude % 6) / lon_diff_scale) + 1
    return row, col

def calc_map_grid(longitude, latitude, scale):
    base_grid = calc_100w_grid(longitude, latitude)
    
    scale_params = {
        "100w": {"code": "", "lon_diff": 6, "lat_diff": 4},
        "50w": {"code": "B", "lon_diff": 3, "lat_diff": 2},
        "25w": {"code": "C", "lon_diff": 1.5, "lat_diff": 1},
        "10w": {"code": "D", "lon_diff": 0.5, "lat_diff": 1/3},
        "5w": {"code": "E", "lon_diff": 0.25, "lat_diff": 1/6},
        "2.5w": {"code": "F", "lon_diff": 0.125, "lat_diff": 1/12},
        "1w": {"code": "G", "lon_diff": 0.0625, "lat_diff": 1/24},
        "5000": {"code": "H", "lon_diff": 0.03125, "lat_diff": 1/48}
    }
    
    if scale == "100w":
        return base_grid
    
    scale_code = scale_params[scale]["code"]
    lon_diff = scale_params[scale]["lon_diff"]
    lat_diff = scale_params[scale]["lat_diff"]
    
    row, col = calc_grid_row_col(longitude, latitude, lon_diff, lat_diff)
    
    row_str = str(row).zfill(3)
    col_str = str(col).zfill(3)
    
    return f"{base_grid}{scale_code}{row_str}{col_str}"
def read_points(file_path):
    points = []
    with open(file_path, 'r') as f:
        for line in f:
            if line.strip():
                id_str, lon_str, lat_str = line.strip().split(',')
                points.append({
                    'id': int(id_str),
                    'longitude': float(lon_str),
                    'latitude': float(lat_str)
                })
    return points

def calc_all_points(points, scale="1w"):
    results = []
    for point in points:
        grid_code = calc_map_grid(point['longitude'], point['latitude'], scale)
        results.append({
            'id': point['id'],
            'longitude': point['longitude'],
            'latitude': point['latitude'],
            'scale': f"1:{scale.replace('w', '万')}",
            'grid_code': grid_code
        })
    return results

def save_to_txt(results, file_path):
    with open(file_path, 'w', encoding='utf-8') as f:
        f.write("点名，经度，纬度，比例尺，图幅编号\n")
        for result in results:
            f.write(f"{result['id']},{result['longitude']},{result['latitude']},{result['scale']},{result['grid_code']}\n")
def save_to_excel(results, file_path):
    df = pd.DataFrame(results)
    df.to_excel(file_path, index=False)

def calc_point_all_scales(point):
    scales = ["100w", "50w", "25w", "10w", "5w", "2.5w", "1w", "5000"]
    results = []
    for scale in scales:
        grid_code = calc_map_grid(point['longitude'], point['latitude'], scale)
        results.append({
            'scale': f"1:{scale.replace('w', '万')}",
            'grid_code': grid_code
        })
    return results

def find_extreme_points(points):
    most_north = max(points, key=lambda x: x['latitude'])
    most_south = min(points, key=lambda x: x['latitude'])
    most_east = max(points, key=lambda x: x['longitude'])
    most_west = min(points, key=lambda x: x['longitude'])
    
    return {
        'north': most_north,
        'south': most_south,
        'east': most_east,
        'west': most_west
    }

def process_data(input_file, txt_output, excel_output):
    points = read_points(input_file)
    
    results_1w = calc_all_points(points, "1w")
    
    save_to_txt(results_1w, txt_output)
    
    excel_data = []
    
    point_1 = next((p for p in points if p['id'] == 1), None)
    if point_1:
        grid_50w = calc_map_grid(point_1['longitude'], point_1['latitude'], "50w")
        grid_25w = calc_map_grid(point_1['longitude'], point_1['latitude'], "25w")
        excel_data.append(["编号为1的点在1：50万比例尺地形图的地图分幅编号", grid_50w])
        excel_data.append(["编号为1的点在1：25万比例尺地形图的地图分幅编号", grid_25w])
    extreme_points = find_extreme_points(points)
    
    north_point = extreme_points['north']
    grid_25w_north = calc_map_grid(north_point['longitude'], north_point['latitude'], "25w")
    excel_data.append(["最北端的点的编号", north_point['id']])
    excel_data.append(["最北端的点的经度", north_point['longitude']])
    excel_data.append(["最北端的点的纬度", north_point['latitude']])
    excel_data.append(["最北端的点在1：25万比例尺地形图的地图分幅编号", grid_25w_north])
    
    south_point = extreme_points['south']
    grid_10w_south = calc_map_grid(south_point['longitude'], south_point['latitude'], "10w")
    excel_data.append(["最南端的点的编号", south_point['id']])
    excel_data.append(["最南端的点的经度", south_point['longitude']])
    excel_data.append(["最南端的点的纬度", south_point['latitude']])
    excel_data.append(["最南端的点在1：10万比例尺地形图的地图分幅编号", grid_10w_south])
    
    east_point = extreme_points['east']
    grid_1w_east = calc_map_grid(east_point['longitude'], east_point['latitude'], "1w")
    excel_data.append(["最东端的点的编号", east_point['id']])
    excel_data.append(["最东端的点的经度", east_point['longitude']])
    excel_data.append(["最东端的点的纬度", east_point['latitude']])
    excel_data.append(["最东端的点在1：1万比例尺地形图的地图分幅编号", grid_1w_east])
    
    west_point = extreme_points['west']
    grid_5000_west = calc_map_grid(west_point['longitude'], west_point['latitude'], "5000")
    excel_data.append(["最西端的点的编号", west_point['id']])
    excel_data.append(["最西端的点的经度", west_point['longitude']])
    excel_data.append(["最西端的点的纬度", west_point['latitude']])
    excel_data.append(["最西端的点在1：5000比例尺地形图的地图分幅编号", grid_5000_west])
    
    point_188 = next((p for p in points if p['id'] == 188), None)
    if point_188:
        scales = ["100w", "50w", "25w", "10w", "5w", "2.5w", "1w", "5000"]
        for scale in scales:
            grid_code = calc_map_grid(point_188['longitude'], point_188['latitude'], scale)
            excel_data.append([f"编号为188的点在1：{scale.replace('w', '万')}比例尺地形图的地图分幅编号", grid_code])
    
    df = pd.DataFrame(excel_data, columns=['说明', '结果输出'])
    df.to_excel(excel_output, index=False)
    
    return {
        "status": "success",
        "message": "计算完成",
        "txt_file": txt_output,
        "excel_file": excel_output
    }

def main():
    script_dir = os.path.dirname(os.path.abspath(__file__))
    data_dir = os.path.join(os.path.dirname(script_dir), "Data")
    
    input_file = os.path.join(data_dir, "points.txt")
    txt_output = os.path.join(data_dir, "队名.txt")
    excel_output = os.path.join(data_dir, "队名.xlsx")
    
    process_data(input_file, txt_output, excel_output)
    
    return {
        "status": "success",
        "message": "计算完成",
        "txt_file": txt_output,
        "excel_file": excel_output
    }

if __name__ == "__main__":
    main() 