using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using Python.Runtime;

namespace C1301
{

    public partial class App : Application
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetDllDirectory(string lpPathName);

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            InitializePythonRuntime();
        }

        private void InitializePythonRuntime()
        {
            try
            {
                string pythonHome = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PythonRuntime");
                string pythonDll = Path.Combine(pythonHome, "python39.dll");

                if (!File.Exists(pythonDll))
                {
                    MessageBox.Show($"Python DLL文件不存在: {pythonDll}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                SetDllDirectory(pythonHome);

                Environment.SetEnvironmentVariable("PYTHONHOME", pythonHome);
                Environment.SetEnvironmentVariable("PYTHONPATH", pythonHome);
                Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", pythonDll);

                PythonEngine.PythonHome = pythonHome;
                Runtime.PythonDLL = pythonDll;

                PythonEngine.Initialize();
                PythonEngine.BeginAllowThreads();

                using (Py.GIL())
                {
                    try
                    {
                        dynamic sys = Py.Import("sys");
                        string scriptsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PythonScripts");
                        sys.path.append(scriptsPath);

                        try
                        {
                            dynamic pd = Py.Import("pandas");
                            Console.WriteLine("pandas已成功加载");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"警告: pandas模块未找到 - {ex.Message}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"错误: 导入sys模块失败 - {ex.Message}");
                    }
                }
                
                Console.WriteLine("Python环境初始化成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化Python运行时失败: {ex.Message}\n\n{ex.StackTrace}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                PythonEngine.Shutdown();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"关闭Python引擎时出错: {ex.Message}");
            }
            base.OnExit(e);
        }
    }
}
