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

namespace OPComander
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        #region On Loaded
        /// <summary>
        /// When the application first opens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(var drive in Directory.GetLogicalDrives())
            {
                var item = new TreeViewItem()
                {

                    Header = drive,
                    Tag = drive
            };
                item.Items.Add(null);
                FolderView.Items.Add(item);
                item.Expanded += Folder_Expanded;

            }
        }

        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)sender;
            if (item.Items.Count != 1 || item.Items[0] != null)
                return;
            item.Items.Clear();
            var fullPath = (string)item.Tag;
            var directories = new List<string>();
            try
            {
                var dirs = Directory.GetDirectories(fullPath);
                if (dirs.Length > 0)
                    directories.AddRange(dirs);
            }
            catch { }
            directories.ForEach(directoryPath =>
            {
                var subitem = new TreeViewItem()
                {
                    Header = GetFileFolderName(directoryPath),
                    Tag = directoryPath

                };
                subitem.Items.Add(null);
                subitem.Expanded += Folder_Expanded;

                item.Items.Add(subitem);
            });
            var files = new List<string>();
            try
            {
                var fs = Directory.GetFiles(fullPath);
                if (fs.Length > 0)
                    files.AddRange(fs);
            }
            catch { }
            files.ForEach(filePath =>
            {
                var subitem = new TreeViewItem()
                {
                    Header = GetFileFolderName(filePath),
                    Tag = filePath

                };


                //subitem.Items.Add(null);
                //subitem.Expanded += Folder_Expanded;
                item.Items.Add(subitem);

            });
        }
            
        #endregion
        
        public static string GetFileFolderName(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;
            var NormalizedPath = path.Replace("/","\\");
            var lastIndex = NormalizedPath.LastIndexOf("\\");
            if (lastIndex <= 0)
                return path;
            return path.Substring(lastIndex+1);

        }
    }
}
