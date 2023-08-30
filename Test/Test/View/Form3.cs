using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;

namespace Test.View
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        Controller.Controller controller = new Controller.Controller();

        private void button1_Click(object sender, EventArgs e)
        {
            //IDとPwdを受け取る
            string id = textBox1.Text;
            string pwd = textBox2.Text;

            //IDとPwd入力チェック
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pwd))
            {
                MessageBox.Show(Common.MessageString.NullUserPass);
                return;
            }

            try
            {
                //IDの存在チェック
                if (!controller.IsUserId(id))
                {
                    MessageBox.Show(Common.MessageString.NotUser);
                    return;
                }
                
                //IDとPwdのひもづきデータのチェック
                if (!controller.IsMatchUserPass(id, pwd))
                {
                    MessageBox.Show(Common.MessageString.DifferentPass);
                    controller.UseInsertLogHistory(0, id);
                    return;
                }
                
                List<DateTime> loginTimesList = controller.LogTimesList(id);
                //ログリストのチェック
                if (controller.CheckLoginTime(loginTimesList))
                {
                    //最後のミスから3分以内かのチェック
                    if (controller.CheckThreeMinutes(loginTimesList))
                    {
                        string lockTime = controller.GetLockTime(loginTimesList).ToString(@"mm\:ss");
                        MessageBox.Show(Common.MessageString.Unlock + lockTime);
                        return;
                    }
                }

                //ログイン成功
                MessageBox.Show(Common.MessageString.Success);
                controller.UseInsertLogHistory(1, id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Common.MessageString.Error);
                Common.ErrorLog errorLog = new Common.ErrorLog();
                errorLog.OutPutError(ex);
            }
        }
    }
}


