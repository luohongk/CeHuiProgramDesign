using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Diagnostics;
using System.Dynamic;
using Python.Runtime;

namespace C1301
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _tempInputFilePath;
        private string _outputTxtPath;
        private string _outputExcelPath;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 设置默认状态
            UpdateStatus("就绪");

            // 设置文件路径
            _tempInputFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "temp_input.txt");
            _outputTxtPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "队名.txt");
            _outputExcelPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "队名.xlsx");

            // 确保Data目录存在
            Directory.CreateDirectory(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data"));
        }

        private void btnLoadFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "文本文件 (*.txt)|*.txt|所有文件 (*.*)|*.*",
                    Title = "选择点数据文件"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    string content = File.ReadAllText(openFileDialog.FileName);
                    txtInput.Text = content;
                    UpdateStatus($"已加载文件: {openFileDialog.FileName}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载文件失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                UpdateStatus("加载文件失败");
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtInput.Clear();
            UpdateStatus("已清空输入");
        }

        private async void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnCalculate.IsEnabled = false;
                UpdateStatus("正在计算...");
                string inputData = txtInput.Text.Trim();
                if (string.IsNullOrEmpty(inputData))
                {
                    MessageBox.Show("请输入点数据", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdateStatus("请输入点数据");
                    btnCalculate.IsEnabled = true;
                    return;
                }
                await Task.Run(() => File.WriteAllText(_tempInputFilePath, inputData));
                string scale = GetSelectedScale();
                var result = await Task.Run(() => CalculateMapGrid(_tempInputFilePath, scale));
                txtResult.Text = result;

                UpdateStatus("计算完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"计算失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                UpdateStatus("计算失败");
            }
            finally
            {
                btnCalculate.IsEnabled = true;
            }
        }

        private string GetSelectedScale()
        {
            ComboBoxItem selectedItem = cmbScale.SelectedItem as ComboBoxItem;
            string scaleText = selectedItem?.Content.ToString() ?? "1:1万";

            Dictionary<string, string> scaleMapping = new Dictionary<string, string>
            {
                { "1:100万", "100w" },
                { "1:50万", "50w" },
                { "1:25万", "25w" },
                { "1:10万", "10w" },
                { "1:5万", "5w" },
                { "1:2.5万", "2.5w" },
                { "1:1万", "1w" },
                { "1:5000", "5000" }
            };

            return scaleMapping.ContainsKey(scaleText) ? scaleMapping[scaleText] : "1w";
        }

        private string CalculateMapGrid(string inputFilePath, string scale)
        {
            try
            {
                using (Py.GIL())
                {
                    dynamic map_grid_calculator = Py.Import("map_grid_calculator");
                    dynamic points = map_grid_calculator.read_points(inputFilePath);
                    dynamic results = map_grid_calculator.calc_all_points(points, scale);

                    StringBuilder resultBuilder = new StringBuilder();
                    resultBuilder.AppendLine("点名，经度，纬度，比例尺，图幅编号");

                    foreach (dynamic result in results)
                    {
                        resultBuilder.AppendLine($"{result["id"]},{result["longitude"]},{result["latitude"]},{result["scale"]},{result["grid_code"]}");
                    }

                    return resultBuilder.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Python脚本执行失败: {ex.Message}", ex);
            }
        }
        private async void btnSaveTxt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string resultText = txtResult.Text;
                
                if (string.IsNullOrEmpty(resultText))
                {
                    MessageBox.Show("没有可保存的结果", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "文本文件 (*.txt)|*.txt",
                    Title = "保存结果到TXT文件",
                    FileName = "队名.txt"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    await Task.Run(() => File.WriteAllText(saveFileDialog.FileName, resultText));
                    UpdateStatus($"已保存到: {saveFileDialog.FileName}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存文件失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                UpdateStatus("保存文件失败");
            }
        }

        private async void btnSaveExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdateStatus("正在生成Excel...");
                btnSaveExcel.IsEnabled = false;

                string inputData = txtInput.Text.Trim();
                if (string.IsNullOrEmpty(inputData))
                {
                    MessageBox.Show("请输入点数据", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdateStatus("请输入点数据");
                    btnSaveExcel.IsEnabled = true;
                    return;
                }
                File.WriteAllText(_tempInputFilePath, inputData);
                var result = await Task.Run(() => GenerateExcel(_tempInputFilePath, _outputTxtPath, _outputExcelPath));
                if (result.status == "success")
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Excel文件 (*.xlsx)|*.xlsx",
                        Title = "保存结果到Excel文件",
                        FileName = "队名.xlsx"
                    };

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        File.Copy(_outputExcelPath, saveFileDialog.FileName, true);
                        MessageBox.Show($"Excel文件已保存: {saveFileDialog.FileName}", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                        UpdateStatus("Excel文件已保存");
                        Process.Start("explorer.exe", $"/select,\"{saveFileDialog.FileName}\"");
                    }
                }
                else
                {
                    MessageBox.Show($"生成Excel失败: {result.message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    UpdateStatus("生成Excel失败");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成Excel失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                UpdateStatus("生成Excel失败");
            }
            finally
            {
                btnSaveExcel.IsEnabled = true;
            }
        }
        private dynamic GenerateExcel(string inputFilePath, string txtOutputPath, string excelOutputPath)
        {
            try
            {
                using (Py.GIL())
                {
                    dynamic map_grid_calculator = Py.Import("map_grid_calculator");

                    dynamic result = map_grid_calculator.process_data(inputFilePath, txtOutputPath, excelOutputPath);

                    return new
                    {
                        status = result["status"].ToString(),
                        message = result["message"].ToString(),
                        txt_file = result["txt_file"].ToString(),
                        excel_file = result["excel_file"].ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Python脚本执行失败: {ex.Message}", ex);
            }
        }
        private void UpdateStatus(string message)
        {
            if (Dispatcher.CheckAccess())
            {
                txtStatus.Text = message;
            }
            else
            {
                Dispatcher.Invoke(() => txtStatus.Text = message);
            }
        }
    }
}
