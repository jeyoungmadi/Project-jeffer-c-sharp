﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Windows.Forms;
using Jeffer;

namespace jeffer.menu_form
{
    public partial class EditMenuForm : Form
    {
        private string sql = "";
        int status;
        DataTable Ori_table = new DataTable();
        DataTable Ori_table_set = new DataTable();
        DataTable Delete_table = new DataTable();

        public EditMenuForm()
        {
            InitializeComponent();
            Delete_table.Columns.Add("PRODUCT_ID");
        }


        private void button_back_Click(object sender, EventArgs e)
        {
            this.Hide();
            Program.listmenuForm = new ListMenuForm();
            Program.listmenuForm.ShowDialog();
            this.Close();
        }

        private void GenerateId_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Update_Load(object sender, EventArgs e)
        {
          
            this.sql = "SELECT MENU_TYPE,MENU_NAME,MENU_PRICE,MENU_ID,MENU_STATUS FROM `menu` WHERE MENU_ID ='" + ListMenuForm.menu_id + "'";
            //WHERE MENU_ID LIKE '%" + ViewMenu.menu_id + "%'
            MySqlCommand cmd = new MySqlCommand(sql, Program.connect);
            Program.connect.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            menutype.Text = reader.GetString("MENU_TYPE");
            txtName.Text = reader.GetString("MENU_NAME");
            txtID.Text = reader.GetString("MENU_ID");
            txtPrice.Text = reader.GetString("MENU_PRICE");
            status = reader.GetInt16("MENU_STATUS");
            Program.connect.Close();
            
            if (status == 1)
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
            }
            else
            {
                radioButton1.Checked = false;
                radioButton2.Checked = true;
                
            }

            if(txtID.Text.Substring(0,3) == "SET")
            {
                Set_menu_add(menutype.Text);
            }
            else
            {
                Table_add(menutype.Text);
            }
                
            
            if (menutype.Text == "Dinein")
            {
                SearchGroup.Text = "Dinein";
                SearchGroup.Enabled = false;
            }
            else
            {
                SearchGroup.Text = "Take-Away";
                SearchGroup.Enabled = false;
            }
            show("");
        }


        private void Table_add(string type)
        {

            //if (count == 1) //รอถามว่า fix เลขไปเลยจะโอเคไหม
            if (type == "Dinein")
            {
                sql = "SELECT PRODUCT_ID,PRODUCT_NAME,INGREDIDENT_QTY FROM (`stock_product` NATURAL JOIN `ingredident`) NATURAL JOIN `menu` WHERE MENU_ID = '" + ListMenuForm.menu_id + "'";
                MySqlCommand cmd = new MySqlCommand(sql, Program.connect);
                Program.connect.Open();

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(Ori_table);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int index = DINEGRID.Rows.Add();
                    DINEGRID.Rows[index].Cells[0].Value = reader.GetString("PRODUCT_ID");
                    DINEGRID.Rows[index].Cells[1].Value = reader.GetString("PRODUCT_NAME");
                    DINEGRID.Rows[index].Cells[2].Value = reader.GetString("INGREDIDENT_QTY");
                }

                Program.connect.Close();
            }
            else if (type == "Take-Away")
            {
                sql = "SELECT PRODUCT_ID,PRODUCT_NAME,INGREDIDENT_QTY FROM (`stock_product` NATURAL JOIN `ingredident`) NATURAL JOIN `menu` WHERE MENU_ID = '" + ListMenuForm.menu_id + "'";
                MySqlCommand cmd = new MySqlCommand(sql, Program.connect);
                Program.connect.Open();

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(Ori_table);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int index = TAKEGRID.Rows.Add();
                    TAKEGRID.Rows[index].Cells[0].Value = reader.GetString("PRODUCT_ID");
                    TAKEGRID.Rows[index].Cells[1].Value = reader.GetString("PRODUCT_NAME");
                    TAKEGRID.Rows[index].Cells[2].Value = reader.GetString("INGREDIDENT_QTY");
                }
                Program.connect.Close();

            }
        }

        private void Set_menu_add(string type)
        { 
            if (type == "Dinein")
            {
                sql = "SELECT setmenu.MENU_ID,MENU_NAME,SETMENU_QTY FROM `menu` INNER JOIN `setmenu` ON menu.MENU_ID = setmenu.MENU_ID WHERE MENU_TYPE = '"+type+"' AND setmenu.SETMENU_ID = '"+txtID.Text+"'";
                MySqlCommand cmd = new MySqlCommand(sql, Program.connect);
                Program.connect.Open();

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(Ori_table);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int index = Menu_dinein.Rows.Add();
                    Menu_dinein.Rows[index].Cells[0].Value = reader.GetString("MENU_ID");
                    Menu_dinein.Rows[index].Cells[1].Value = reader.GetString("MENU_NAME");
                    Menu_dinein.Rows[index].Cells[2].Value = reader.GetString("SETMENU_QTY");
                }

                Program.connect.Close();
            }
            else if (type == "Take-Away")
            {
                sql = "SELECT setmenu.MENU_ID,MENU_NAME,SETMENU_QTY FROM `menu` INNER JOIN `setmenu` ON menu.MENU_ID = setmenu.MENU_ID WHERE MENU_TYPE = '" + type + "' AND setmenu.SETMENU_ID = '" + txtID.Text + "'";
                MySqlCommand cmd = new MySqlCommand(sql, Program.connect);
                Program.connect.Open();

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(Ori_table);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int index = Menu_take.Rows.Add();
                    Menu_take.Rows[index].Cells[0].Value = reader.GetString("MENU_ID");
                    Menu_take.Rows[index].Cells[1].Value = reader.GetString("MENU_NAME");
                    Menu_take.Rows[index].Cells[2].Value = reader.GetString("SETMENU_QTY");
                }

                Program.connect.Close();

            }
        }


        private void Search_TextChanged(object sender, EventArgs e)
        {
            show(Search.Text);

        }
        private void show(string search)
        {
            if (search == "")
            {
                if (txtID.Text.Substring(0, 3) == "SET")
                {
                    Menu_view.Visible = true;
                    Table_view.Visible = false;
                    DINEGRID.Visible = false;
                    TAKEGRID.Visible = false;
                    Menu_dinein.Visible = true;
                    Menu_take.Visible = true;

                    if (SearchGroup.Text == "Dinein")
                    {
                        this.sql = "SELECT MENU_ID,MENU_NAME,MENU_PRICE FROM `menu` WHERE MENU_TYPE = 'Dinein' AND MENU_STATUS = '1'";                      
                    }
                    else if (SearchGroup.Text == "Take-Away")
                    {
                        this.sql = "SELECT MENU_ID,MENU_NAME,MENU_PRICE FROM `menu` WHERE MENU_TYPE = 'Take-Away' MENU_STATUS = '1'";
                    }

                    this.Menu_view.DataSource = Program.SQLlist(this.sql);

                }
                else
                {
                    Menu_view.Visible = false;
                    Table_view.Visible = true;
                    DINEGRID.Visible = true;
                    TAKEGRID.Visible = true;
                    Menu_dinein.Visible = false;
                    Menu_take.Visible = false;

                    this.sql = "SELECT PRODUCT_ID,PRODUCT_NAME,PRODUCT_UNIT FROM `stock_product`";
                    Table_view.DataSource = Program.SQLlist(this.sql);
                }
            }
            else
            {

                if (txtID.Text.Substring(0, 3) == "SET")
                {
                    Menu_view.Visible = true;
                    Table_view.Visible = false;
                    DINEGRID.Visible = false;
                    TAKEGRID.Visible = false;
                    Menu_dinein.Visible = true;
                    Menu_take.Visible = true;
                    if (SearchGroup.Text == "Dinein")
                    {
                        this.sql = "SELECT MENU_ID,MENU_NAME,MENU_PRICE FROM `menu` WHERE MENU_TYPE = 'Dinein' AND MENU_NAME LIKE '%" + search + "%' AND MENU_STATUS = '1'";                       
                    }

                    else if (SearchGroup.Text == "Take-Away")
                    {
                        this.sql = "SELECT MENU_ID,MENU_NAME,MENU_PRICE FROM `menu` WHERE MENU_TYPE = 'Take-Away' AND  MENU_NAME LIKE '%" + search + "%' AND MENU_STATUS ='1'";
                    }

                    this.Menu_view.DataSource = Program.SQLlist(this.sql);

                }
                else
                {
                    Menu_view.Visible = false;
                    Table_view.Visible = true;
                    DINEGRID.Visible = true;
                    TAKEGRID.Visible = true;
                    Menu_dinein.Visible = false;
                    Menu_take.Visible = false;

                    this.sql = "SELECT PRODUCT_ID,PRODUCT_NAME,PRODUCT_UNIT FROM `stock_product` WHERE PRODUCT_NAME LIKE '%" + search + "%'";                   
                    Table_view.DataSource = Program.SQLlist(this.sql);
                }

            }
        }

        private void SearchGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            show("");
        }

        private void DINEGRID_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3 && e.RowIndex != -1)
            {
                Delete_table.Rows.Add(DINEGRID.Rows[e.RowIndex].Cells[0].Value);
                DINEGRID.Rows.RemoveAt(e.RowIndex);
            }
        }

        private void TAKEGRID_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3 && e.RowIndex != -1)
            {
                Delete_table.Rows.Add(TAKEGRID.Rows[e.RowIndex].Cells[0].Value);
                TAKEGRID.Rows.RemoveAt(e.RowIndex);

            }

        }



        private void button_update_Click(object sender, EventArgs e)
        {
            if (txtID != null || txtName != null || txtPrice != null)
            {
                if (menutype.Text == "Dinein")
                {
                    update_menu(menutype.Text);
                    update_product(menutype.Text);
                    MessageBox.Show("UPDATE SUSCESS!!", "COMPLETE !");
                    this.button_back_Click(sender, e);
                }
                else if (menutype.Text == "Take-Away")
                {
                    update_menu(menutype.Text);
                    update_product(menutype.Text);
                    MessageBox.Show("UPDATE SUSCESS!!", "COMPLETE !");
                    this.button_back_Click(sender, e);
                }
                else
                {
                    MessageBox.Show("Please check your informations", "Wrong informations !");
                }
            }
            else
            {
                MessageBox.Show("Please check your informations", "Wrong informations !");
            }
        }

        private int check_status()
        {
            if (radioButton1.Checked == true)
                return 1;
            return 0;
        }

        private void update_menu(string type)
        {
            int status = check_status();
            if (type == "Dinein")
            {
                this.updateMenuDinein();
            }
            else if (type == "Take-Away")
            {             
                this.updateMenuDinein();          
                this.updateMenuTakeAway();
            }

        }

        //อัพเดทเมนูสำหรับกินที่ร้าน
        private void updateMenuDinein()
        {
            this.sql = "UPDATE `menu` SET MENU_NAME = '" + txtName.Text + "',MENU_PRICE = '" + txtPrice.Text + "' ,MENU_TYPE = 'Dinein',MENU_STATUS = " + status + " WHERE MENU_ID = '" + txtID.Text + "'";
            Program.sqlOther(this.sql);
        }

        //อัพเดทเมนูสำหรับกลับบ้าน
        private void updateMenuTakeAway()
        {
            sql = "UPDATE `menu` SET MENU_NAME = '" + txtName.Text + "',MENU_PRICE = '" + txtPrice.Text + "' ,MENU_TYPE = 'Take-Away',MENU_STATUS = " + status + "  WHERE MENU_ID = '" + txtID.Text + "'";
            Program.sqlOther(this.sql);
        }

        private void update_product(string type)
        {

            if (type == "Dinein")
            {
                if (txtID.Text.Substring(0, 3) == "SET")
                {
                    int flag = 0;
                    int index = 0;
                    foreach (DataRow row1 in Delete_table.Rows)
                    {
                        this.sql = "DELETE FROM `setmenu` WHERE SETMENU_ID = '" + txtID.Text + "' AND MENU_ID = '" + row1[0].ToString()+ "'";
                        Program.sqlOther(this.sql);
                    }
                    foreach (DataGridViewRow row in Menu_dinein.Rows)
                    {
                        flag = 0;
                        foreach (DataRow row1 in Ori_table.Rows)
                        {
                            if (row.Cells[0].Value.ToString() == row1[0].ToString())
                            {
                                MessageBox.Show("CHECK");
                                this.sql = "UPDATE `setmenu` SET SETMENU_QTY = " + row.Cells[2].Value + " WHERE SETMENU_ID = '" + txtID.Text + "' AND MENU_ID =  '" + row.Cells[0].Value + "'";
                                Program.sqlOther(this.sql);
                                index = Ori_table.Rows.IndexOf(row1);
                                Ori_table.Rows[index][2] = 0;
                                flag = 1;
                                break;
                            }

                        }
                        if (flag == 0)
                        {
                            this.sql = "INSERT INTO `setmenu` (SETMENU_ID,MENU_ID,SETMENU_QTY) VALUES ('" + txtID.Text + "','" + row.Cells[0].Value.ToString() + "' ,'" + row.Cells[2].Value + "')";
                            Program.sqlOther(this.sql);
                        }
                    }

                }
                else
                {
                    int flag = 0;
                    int index = 0;
                    foreach (DataRow row1 in Delete_table.Rows)
                    {
                        this.sql = "DELETE FROM `ingredident` WHERE MENU_ID = '" + txtID.Text + "' AND PRODUCT_ID = '" + row1[0].ToString() +"'";
                        Program.sqlOther(this.sql);
                    }
                    foreach (DataGridViewRow row in DINEGRID.Rows)
                    {
                        flag = 0;
                        foreach (DataRow row1 in Ori_table.Rows)
                        {
                            if (row.Cells[0].Value.ToString() == row1[0].ToString())
                            {
                                this.sql = "UPDATE `ingredident` SET INGREDIDENT_QTY = '" + row.Cells[2].Value + "' WHERE MENU_ID = '" + txtID.Text + "' AND PRODUCT_ID =  '" + row.Cells[0].Value + "'";
                                Program.sqlOther(this.sql);
                                index = Ori_table.Rows.IndexOf(row1);
                                Ori_table.Rows[index][2] = 0;
                                flag = 1;
                                break;
                            }

                        }
                        if (flag == 0)
                        {
                            this.sql = "INSERT INTO `ingredident` (MENU_ID,PRODUCT_ID,INGREDIDENT_QTY) VALUES ('" + txtID.Text + "','" + row.Cells[0].Value + "' ,'" + row.Cells[2].Value + "')";
                            Program.sqlOther(this.sql);
                        }
                    }
                }


            }
            else if (type == "Take-Away")
            {
                if (txtID.Text.Substring(0, 3) == "SET")
                {
                    int flag = 0;
                    int index = 0;
                    foreach (DataRow row1 in Delete_table.Rows)
                    {
                        this.sql = "DELETE FROM `setmenu` WHERE SETMENU_ID = '" + txtID.Text + "' AND MENU_ID = '" + row1[0].ToString()+"'";
                        Program.sqlOther(this.sql);
                    }
                    foreach (DataGridViewRow row in Menu_take.Rows)
                    {
                        flag = 0;
                        foreach (DataRow row1 in Ori_table.Rows)
                        {
                            if (row.Cells[0].Value.ToString() == row1[0].ToString())
                            {
                                this.sql = "UPDATE `setmenu` SET SETMENU_QTY = " + row.Cells[2].Value + " WHERE SETMENU_ID = '" + txtID.Text + "' AND MENU_ID =  '" + row.Cells[0].Value + "'";
                                Program.sqlOther(this.sql);
                                index = Ori_table.Rows.IndexOf(row1);
                                Ori_table.Rows[index][2] = 0;
                                flag = 1;
                                break;
                            }

                        }
                        if (flag == 0)
                        {
                            this.sql = "INSERT INTO `setmenu` (SETMENU_ID,MENU_ID,SETMENU_QTY) VALUES ('" + txtID.Text + "','" + row.Cells[0].Value + "' ,'" + row.Cells[2].Value + "')";
                            Program.sqlOther(this.sql);
                        }
                    }
                }
                else
                {
                    int flag = 0;
                    int index = 0;
                    foreach (DataRow row1 in Delete_table.Rows)
                    {
                        this.sql = "DELETE FROM `ingredident` WHERE MENU_ID = '" + txtID.Text + "' AND PRODUCT_ID = '" + row1[0].ToString()+"'";
                        Program.sqlOther(this.sql);
                    }
                    foreach (DataGridViewRow row in TAKEGRID.Rows)
                    {
                        flag = 0;
                        foreach (DataRow row1 in Ori_table.Rows)
                        {
                            if (row.Cells[0].Value.ToString() == row1[0].ToString())
                            {
                                this.sql = "UPDATE `ingredident` SET INGREDIDENT_QTY = " + row.Cells[2].Value + " WHERE MENU_ID = '" + txtID.Text + "' AND PRODUCT_ID =  '" + row.Cells[0].Value + "'";
                                Program.sqlOther(this.sql);
                                index = Ori_table.Rows.IndexOf(row1);
                                Ori_table.Rows[index][2] = 0;
                                flag = 1;
                                break;
                            }

                        }
                        if (flag == 0)
                        {
                            this.sql = "INSERT INTO `ingredident` (MENU_ID,PRODUCT_ID,INGREDIDENT_QTY) VALUES ('" + txtID.Text + "','" + row.Cells[0].Value + "' ,'" + row.Cells[2].Value + "')";
                            Program.sqlOther(this.sql);
                        }
                    }
                }
            }

        }

        private void Search_MouseClick(object sender, MouseEventArgs e)
        {
            Search.Text = "";
            Search.ForeColor = System.Drawing.Color.Black;
        }
        private void Table_view_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex != -1)//check click
            {
                string menu_id = Table_view.Rows[e.RowIndex].Cells[1].Value.ToString();
                string menu_name = Table_view.Rows[e.RowIndex].Cells[2].Value.ToString();
                if (SearchGroup.Text == "Dinein")
                {
                    DINEGRID = Program.check_duplicate(DINEGRID, menu_id, menu_name);
                }
                else if (SearchGroup.Text == "Take-Away")
                {
                    TAKEGRID = Program.check_duplicate(TAKEGRID, menu_id, menu_name);
                }
            }
        }
        private void Menu_view_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex != -1)//check click
            {
                string menu_id = Menu_view.Rows[e.RowIndex].Cells[1].Value.ToString();
                string menu_name = Menu_view.Rows[e.RowIndex].Cells[2].Value.ToString();
                if (SearchGroup.Text == "Dinein")
                {
                    Menu_dinein = Program.check_duplicate(Menu_dinein, menu_id, menu_name);
                }
                else if (SearchGroup.Text == "Take-Away")
                {
                    Menu_take = Program.check_duplicate(Menu_take, menu_id, menu_name);
                }
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            Time_1.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
        }

        private void Menu_dinein_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3 && e.RowIndex != -1)
            {
                Delete_table.Rows.Add(Menu_dinein.Rows[e.RowIndex].Cells[0].Value);
                Menu_dinein.Rows.RemoveAt(e.RowIndex);
                
            }
        }

        private void Menu_take_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3 && e.RowIndex != -1)
            {
                Delete_table.Rows.Add(Menu_take.Rows[e.RowIndex].Cells[0].Value);
                Menu_take.Rows.RemoveAt(e.RowIndex);
                
            }
        }
    }
}
