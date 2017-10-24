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
using System.Windows.Forms;

namespace SourceModelFixer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private List<string> GetFiles(string IN_DIR)
        {

            List<string> FileList = Directory.GetFiles(IN_DIR).ToList<string>(); // List all the files
            List<string> SubDir = Directory.GetDirectories(IN_DIR).ToList<string>(); // Get Sub-Directories

            foreach (string DIR in SubDir)
                FileList.AddRange(GetFiles(DIR)); // Recursively add all the sub-directories files too

            return FileList;
        }

        private List<string> FilterVtx(List<string> FileList)
        {
            List<string> tmp = new List<string>();
            foreach (string File in FileList) // filter out any non-vtx files.
                if (File.Contains(".vtx"))
                    tmp.Add(File);

            FileList.Clear();
            foreach (string File in tmp)
            {
                string[] FileName = File.Split('.'); // seperate into just file name + extensions

                if (FileName.Length == 3) // 3 entries, therefore it's a .dx90.vtx file
                    if (tmp.Contains(FileName[0] + ".vtx")) // there is a .dx90.vtx and a .vtx
                        FileList.Add(FileName[0] + ".vtx");
            }

            return FileList;
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog VtxDir = new FolderBrowserDialog();
            if (VtxDir.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string Dir = VtxDir.SelectedPath;
                DirectoryEntry.Text = Dir;
            }
        }

        private void GetFiles_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(DirectoryEntry.Text))
            {
                try
                {
                    List<string> fileList = GetFiles(DirectoryEntry.Text);
                    fileList = FilterVtx(fileList);

                    foreach (string file in fileList)
                        Console.Text += file + '\n';

                    FileListGlob = fileList;
                } catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Error getting files: " + ex.ToString());
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Error: please select a directory containing .vtx and .dx90.vtx files.");
            }
        }

        private void GitHub_MouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://github.com/xNWP");
        }

        private void Twitter_MouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://twitter.com/ThatNWP");
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if(System.Windows.MessageBox.Show("Are you sure you want to continue? " +
                "This will delete all files with a .vtx extension that also have a .dx90.vtx extension.",
                "Delete .vtx",
                MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
            {
                try
                {
                    foreach (string file in FileListGlob)
                        File.Delete(file);

                    System.Windows.MessageBox.Show("Successfully Deleted "+ FileListGlob.Count().ToString() + " files!" );
                } catch (Exception exx)
                {
                    System.Windows.MessageBox.Show("Error deleting files: " + exx.ToString());
                }
            }
        }

        private List<string> FileListGlob;
    }
}
