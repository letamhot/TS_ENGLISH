using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using TS_Project.Model;

namespace TS_Project
{
    public partial class ucThuThach : UserControl
    {
        private Socket _socket;
        private int _doiid = 0;
        private int _cauhoiid = 0;
        private bool _isStart;
        private int _cuocthiid = 0;
        private int thoiGianTraLoi = 0;
        private bool ck = false;
        string b;
        private string currentPath = Directory.GetCurrentDirectory();
        QuaMienDiSanEntities _entities = new QuaMienDiSanEntities();
        public ucThuThach()
        {
            InitializeComponent();
        }
        public ucThuThach(Socket sock, int doiid, int cauhoiid, bool start, int cuocthiid)
        {
            InitializeComponent();
            _socket = sock;
            _doiid = doiid;
            _cauhoiid = cauhoiid;
            _isStart = start;
            _cuocthiid = cuocthiid;
            loadUC();


        }

        private void loadUC()
        {

            if (_cauhoiid == 0)
            {
                VisibleGui();
                //lblGioiThieu.Text = "Có 04 câu hỏi về địa danh hay nhân vật xưa và nay nổi tiếng trong các lĩnh vực, những sự kiện mang ý nghĩa chính trị, kinh tế, xã hội của quê hương Quảng Bình. Các thí sinh hãy phát huy tối đa khả năng phán đoán, nhanh tay, nhanh mắt trả lời mỗi câu hỏi trong 30’’.\nTrả lời từ 1-10’’ được 30 điểm, từ 11-20’’ được 20 điểm và từ 21-30’’ được 10 điểm.\nNếu câu trả lời sai chính tả, thí sinh sẽ không được tính điểm.\nĐiểm tối đa cho mỗi thí sinh ở phần thi này là 120 điểm.";

            }
            else
            {
                EnabledGui();
                if (_isStart)
                {
                    //Thread.Sleep(1500);
                    tmKhamPha.Enabled = true;
                    txtCauTraLoi.Enabled = true;
                    pbGui.Enabled = true;
                    displayUCKhamPha(_cauhoiid);
                    if (txtCauTraLoi.InvokeRequired)
                    {
                        this.BeginInvoke((Action)(() =>
                        {
                            System.Windows.Forms.Timer focusTimer = new System.Windows.Forms.Timer();
                            focusTimer.Interval = 100; // 100 ms
                            focusTimer.Tick += (s, e) =>
                            {
                                focusTimer.Stop();
                                txtCauTraLoi.Focus();
                                txtCauTraLoi.Select();
                            };
                            focusTimer.Start();

                        }));

                    }
                    else
                    {
                        System.Windows.Forms.Timer focusTimer = new System.Windows.Forms.Timer();
                        focusTimer.Interval = 100; // 100 ms
                        focusTimer.Tick += (s, e) =>
                        {
                            focusTimer.Stop();
                            txtCauTraLoi.Focus();
                            txtCauTraLoi.Select();
                        };
                        focusTimer.Start();
                    }



                }
                else
                {
                    txtCauTraLoi.Enabled = false;
                    pbGui.Enabled = false;
                    OnTimeUp();
                    pbGui.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\send_in.png");
                    displayUCKhamPha(_cauhoiid);
                }
            }
        }

        private void ucKhamPha_Load(object sender, EventArgs e)
        {

        }

        public void VisibleGui()
        {
            //lblCauTraLoi.Visible = false;
            lblThele.Visible = true;
            //lblGioiThieu.Visible = true;
            txtCauTraLoi.Visible = false;
            lblTGTL.Visible = false;
            lblThoiGianTraLoi.Visible = false;
            pbDapAn.Visible = false;
            pbGui.Visible = false;

        }

        public void EnabledGui()
        {
            //lblCauTraLoi.Visible = true;
            //lblNoiDungCauHoi.Visible = true;
            //lblGioiThieu.Visible = false;
            lblThele.Visible = true;
            txtCauTraLoi.Visible = true;
            lblTGTL.Visible = true;
            lblThoiGianTraLoi.Visible = true;
            pbDapAn.Visible = true;
            pbGui.Visible = true;
        }
        //private Button _draggedButton; // Lưu button đang được kéo
        private Button _draggedButton;
        private PictureBox _dragPreview;
        private Point _dragOffset;
        public void displayUCKhamPha(int cauHoiId)
        {
            EnabledGui();
            ds_cauhoithuthach khamPha = _entities.ds_cauhoithuthach.Find(cauHoiId);
            _entities.Entry(khamPha).Reload(); // ⚠️ Nạp lại từ DB

            if (khamPha != null)
            {
                //Hiển thị loại câu hỏi theo vị trí
                if (khamPha.vitri == 1 || khamPha.vitri == 2)
                {
                    lblThele.Text = "Question " + khamPha.vitri + ": Rearrange the following words or phrases to make a complete sentence";

                }
                else if (khamPha.vitri == 3 || khamPha.vitri == 4)
                {
                    lblThele.Text = "Question " + khamPha.vitri + ": Rearrange the following sentences to make a meaningful conversation";

                }
                else
                {
                    lblThele.Text = "Question " + khamPha.vitri + ": Rearrange the following sentences to make a meaningful paragraph";

                }
                // Tối ưu hiển thị, tránh nháy bằng cách bật DoubleBuffered
                EnableDoubleBuffering(flowPanelSentences);
                // Cấu hình FlowLayoutPanel
                //flowPanelSentences.SuspendLayout();
                flowPanelSentences.Controls.Clear();
                flowPanelSentences.FlowDirection = FlowDirection.TopDown;
                flowPanelSentences.WrapContents = false;
                flowPanelSentences.AutoScroll = true;
                flowPanelSentences.Padding = new Padding(5);
                flowPanelSentences.AllowDrop = true; // Bắt buộc phải có dòng này

                string[] sentences = khamPha.noidung.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                string[] answerLabels = { "A", "B", "C", "D", "E" };
                txtCauTraLoi.Text = string.Join("", answerLabels.Take(sentences.Length));

                // Thiết kế màu sắc và font
                Color primaryColor = Color.FromArgb(52, 152, 219);
                Color hoverColor = Color.FromArgb(41, 128, 185);
                Color dragOverColor = Color.FromArgb(46, 204, 113);
                Color textColor = Color.White;
                Font btnFont = new Font("Segoe UI", 10, FontStyle.Bold);

                int buttonWidth = flowPanelSentences.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 10;

                foreach (int i in Enumerable.Range(0, sentences.Length))
                {
                    string buttonText = answerLabels[i] + ". " + sentences[i].Trim();

                    Button btn = new Button
                    {
                        Text = buttonText,
                        Font = btnFont,
                        ForeColor = textColor,
                        BackColor = _isStart ? primaryColor : Color.LightGray,
                        FlatStyle = FlatStyle.Flat,
                        Width = buttonWidth,
                        Height = 60,
                        Margin = new Padding(0, 0, 0, 5),
                        TextAlign = ContentAlignment.MiddleLeft,
                        Padding = new Padding(10, 5, 5, 5),
                        Cursor = _isStart ? Cursors.Hand : Cursors.Default,
                        Tag = answerLabels[i].ToUpper(), // Store as uppercase
                        Enabled = _isStart,
                        AllowDrop = true // Bắt buộc phải có dòng này
                    };

                    // Thiết kế flat với bo góc
                    btn.FlatAppearance.BorderSize = 0;
                    btn.FlatAppearance.MouseOverBackColor = hoverColor;
                    btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(32, 102, 155);

                    // Tính toán lại chiều cao dựa trên nội dung
                    Size textSize = TextRenderer.MeasureText(btn.Text, btn.Font,
                        new Size(btn.Width - btn.Padding.Horizontal, int.MaxValue));
                    btn.Height = Math.Max(60, textSize.Height + btn.Padding.Vertical + 15);

                    // Thêm hiệu ứng bo góc
                    btn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btn.Width, btn.Height, 15, 15));

                    if (_isStart)
                    {
                        // Sự kiện drag & drop
                        btn.MouseDown += Btn_MouseDown;
                        btn.DragOver += Btn_DragOver;
                        btn.DragLeave += Btn_DragLeave;
                        btn.DragDrop += Btn_DragDrop;
                        btn.QueryContinueDrag += Btn_QueryContinueDrag;

                        // Hiệu ứng hover
                        btn.MouseEnter += (s, e) => btn.BackColor = hoverColor;
                        btn.MouseLeave += (s, e) => btn.BackColor = primaryColor;
                    }

                    flowPanelSentences.Controls.Add(btn);
                }
            }
        }
        private void EnableDoubleBuffering(Control control)
        {
            typeof(Control).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, control, new object[] { true });
        }

        private void Btn_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _draggedButton = sender as Button;
                // Highlight các button còn lại
                foreach (Button btn in flowPanelSentences.Controls.OfType<Button>())
                {
                    if (btn != _draggedButton)
                        btn.BackColor = Color.FromArgb(46, 204, 113); // Màu drag highlight (Xanh lá)
                }
                // Tạo ảnh "ghost"
                Bitmap bmp = new Bitmap(_draggedButton.Width, _draggedButton.Height);
                _draggedButton.DrawToBitmap(bmp, new Rectangle(Point.Empty, _draggedButton.Size));
                bmp = CreateTransparentBitmap(bmp); // 👈 thêm dòng này

                // Hiển thị ảnh dưới con trỏ
                _dragPreview = new PictureBox
                {
                    Image = bmp,
                    Size = _draggedButton.Size,
                    Location = _draggedButton.PointToScreen(Point.Empty),
                    BackColor = Color.Transparent
                };
                _dragOffset = new Point(e.X, e.Y);

                //_dragPreview.TopMost = true;
                _dragPreview.BringToFront();
                _dragPreview.Parent = this.FindForm();
                _dragPreview.Show();

                _dragPreview.DoDragDrop(_draggedButton, DragDropEffects.Move);
            }
        }
        private Bitmap CreateTransparentBitmap(Bitmap original)
        {
            Bitmap transparentBmp = new Bitmap(original.Width, original.Height);
            using (Graphics g = Graphics.FromImage(transparentBmp))
            {
                ColorMatrix matrix = new ColorMatrix();
                matrix.Matrix33 = 0.7f; // Độ trong suốt (0.7 = 70%)
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            }
            return transparentBmp;
        }

        private void Btn_DragOver(object sender, DragEventArgs e)
        {
            if (_dragPreview != null)
            {
                Point cursorPos = Cursor.Position;
                _dragPreview.Location = new Point(cursorPos.X - _dragOffset.X, cursorPos.Y - _dragOffset.Y);
            }
            e.Effect = DragDropEffects.Move;
        }


        private void Btn_DragLeave(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.BackColor = Color.FromArgb(52, 152, 219); // Màu mặc định
        }

        private void Btn_DragDrop(object sender, DragEventArgs e)
        {
            Button targetButton = (Button)sender;

            if (_draggedButton != null && targetButton != _draggedButton)
            {
                int draggedIndex = flowPanelSentences.Controls.IndexOf(_draggedButton);
                int targetIndex = flowPanelSentences.Controls.IndexOf(targetButton);

                flowPanelSentences.Controls.SetChildIndex(_draggedButton, targetIndex);

                UpdateAnswerOrder();
            }

            // Dọn ảnh kéo
            if (_dragPreview != null)
            {
                _dragPreview.Dispose();
                _dragPreview = null;
            }
            // 👉 Focus lại sau khi kéo thả
            this.BeginInvoke((Action)(() =>
            {
                if (txtCauTraLoi.Enabled)
                {
                    txtCauTraLoi.Focus();
                    txtCauTraLoi.Select();
                }
            }));
        }

        private void Btn_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            // Reset màu nền nếu hủy drag
            if (e.Action == DragAction.Cancel || e.Action == DragAction.Drop)
            {
                foreach (Button btn in flowPanelSentences.Controls.OfType<Button>())
                {
                    btn.BackColor = Color.FromArgb(52, 152, 219); // Màu xanh ban đầu
                }
            }
            // 👉 Focus lại txtCauTraLoi
            if (txtCauTraLoi.CanFocus)
            {
                txtCauTraLoi.Focus();
                txtCauTraLoi.Select();
            }
        }

        private void UpdateAnswerOrder()
        {
            txtCauTraLoi.Text = string.Join("", flowPanelSentences.Controls.OfType<Button>()
                .Select(btn => btn.Tag.ToString().ToUpper()));
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);
        private void tmKhamPha_Tick(object sender, EventArgs e)
        {

            if (thoiGianTraLoi < 30)
            {
                if (ck == true)
                {
                    txtCauTraLoi.Text = b;
                }
                thoiGianTraLoi++;

            }
            else
            {
                tmKhamPha.Stop();
                txtCauTraLoi.Enabled = false;

                pbGui.Enabled = false;
                pbGui.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\send_in.png");
                // Thêm dòng này để tự động ẩn nội dung khi hết giờ
                OnTimeUp();
            }

        }
        private void SendEvent(string str)
        {

            // Check we are connected
            if (_socket == null || !_socket.Connected)
            {
                MessageBox.Show(this, "Must be connected to Send a message");
                return;
            }
            // Read the message from the text box and send it
            try
            {
                // Convert to byte array and send.
                Byte[] byteDateLine = Encoding.ASCII.GetBytes(str.ToCharArray());
                _socket.Send(byteDateLine, byteDateLine.Length, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Send lenh dieu khien loi!");
            }

        }
        private void txtCauTraLoi_TextChanged(object sender, EventArgs e)
        {
            string inputOrder = txtCauTraLoi.Text.Trim().ToUpper();
            List<Button> buttons = flowPanelSentences.Controls.OfType<Button>().ToList();

            if (inputOrder.Length == buttons.Count)
            {
                var orderedButtons = inputOrder.Select(label =>
                    buttons.FirstOrDefault(btn => btn.Text.StartsWith(label + ".")))
                    .Where(btn => btn != null).ToList();

                for (int i = 0; i < orderedButtons.Count; i++)
                {
                    flowPanelSentences.Controls.SetChildIndex(orderedButtons[i], i);
                }
            }
        }
        private void pbGui_Click(object sender, EventArgs e)
        {

            VerifyAnswer();
        }

        private void txtCauTraLoi_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                ck = true;
                VerifyAnswer();

            }
        }

        private void VerifyAnswer()
        {
            if (txtCauTraLoi.Text.Trim() == "")
            {
                MessageBox.Show("Yêu cầu nhập trước khi nhấn nút gửi");
                txtCauTraLoi.Focus();
            }
            else
            {
                txtCauTraLoi.Enabled = false;

                ds_diem diem = new ds_diem();
                diem.cuocthiid = _cuocthiid;
                diem.doiid = _doiid;
                diem.cautraloi = txtCauTraLoi.Text.ToUpper();
                diem.cauhoiid = _cauhoiid;
                diem.phanthiid = 3;
                diem.thoigiantraloi = thoiGianTraLoi;
                _entities.ds_diem.Add(diem);
                _entities.SaveChanges();

                b = diem.cautraloi;
                lblThoiGianTraLoi.Text = thoiGianTraLoi.ToString() + " giây";
                pbGui.Enabled = false;
                pbGui.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\send_in.png");
                SendEvent(_doiid + ",cli,playthuthach," + diem.diemid);

                // Ẩn nội dung khi người dùng gửi câu trả lời
                HideContent();
            }
        }

        // Hàm ẩn toàn bộ các nút nội dung và tắt kéo/thả
        private void HideContent()
        {
            foreach (Button btn in flowPanelSentences.Controls.OfType<Button>())
            {
                btn.AllowDrop = false; // Tắt kéo/thả
                btn.Enabled = false;   // Vô hiệu hóa nút
                btn.BackColor = Color.LightGray; // Đổi màu để thể hiện trạng thái không hoạt động
                btn.Cursor = Cursors.Default; // Đổi con trỏ về mặc định
            }
        }
        // Khi hết thời gian, tự động ẩn nội dung
        private void OnTimeUp()
        {
            HideContent();
        }

    }
}
