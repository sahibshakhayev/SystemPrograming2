using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace HW2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*";
            openFileDialog.Title = "Select a file";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "All files (*.*)|*.*";
            saveFileDialog.Title = "Save the file as";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = saveFileDialog.FileName;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please select a source file.");
                return;
            }

            if (textBox2.Text == "")
            {
                MessageBox.Show("Please specify a destination path.");
                return;
            }

            Thread copyThread = new Thread(() =>
            {
                try
                {
                    using (FileStream sourceStream = new FileStream(textBox1.Text, FileMode.Open, FileAccess.Read))
                    using (FileStream destinationStream = new FileStream(textBox2.Text, FileMode.Create, FileAccess.Write))
                    {
                        byte[] buffer = new byte[4096];
                        long totalBytes = sourceStream.Length;
                        long copiedBytes = 0;

                        while (copiedBytes < totalBytes)
                        {
                            int bytesToCopy = (int)Math.Min(buffer.Length, totalBytes - copiedBytes);
                            int bytesRead = sourceStream.Read(buffer, 0, bytesToCopy);
                            destinationStream.Write(buffer, 0, bytesRead);
                            copiedBytes += bytesRead;

                            int progressPercentage = (int)((double)copiedBytes / totalBytes * 100);

                            progressBar1.Invoke((MethodInvoker)(() =>
                            {
                                progressBar1.Value = progressPercentage;
                                label3.Text = $"{progressPercentage} %";
                            }));
                        }
                    }

                    MessageBox.Show("File copy completed.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            });

            copyThread.Start();
        }
    }
}