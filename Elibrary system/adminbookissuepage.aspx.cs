using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Elibrary_system
{
    public partial class adminbookissuepage : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1.DataBind();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            getnames();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (checkifbookexists() && checkifmemberexists())
            {
                if (checkifissueentryexists())
                {
                    Response.Write("<script>alert('this member already have this book');</script>");
                }
                else
                {
                    issuebooks();
                }
            }

            else
            {
                Response.Write("<script>alert('wrong book ID or member id');</script>");
            }
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            if (checkifbookexists() && checkifmemberexists())
            {
                if (checkifissueentryexists())
                {

                    returnbook();//Response.Write("<script>alert('this member already have this book');</script>");
                }
                else
                {
                    Response.Write("<script>alert('this entry doesnot exists');</script>");
                }
            }

            else
            {
                Response.Write("<script>alert('wrong book ID or member id');</script>");
            }
        }
        void returnbook()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("DELETE from book_issue_tbl WHERE book_id='" + TextBox1.Text.Trim() + "' AND member_id='" + TextBox2.Text.Trim() + "'", con);

                int result = cmd.ExecuteNonQuery();
               
                if (result > 0)
                {
           cmd = new SqlCommand("update book_master_tbl set current_stock=current_stock+1 WHERE book_id='" + TextBox1.Text.Trim() + "'", con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    Response.Write("<script>alert('book returned Successfully');</script>");
                    
                    GridView1.DataBind();
                    con.Close();

                }
                else
                {
                    Response.Write("<script>alert('invalid details');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }






        //userdefiend codes
        void issuebooks()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("INSERT INTO book_issue_tbl(member_id,member_name,book_id,book_name,issue_date,due_date) values(@member_id,@member_name,@book_id,@book_name,@issue_date,@due_date)", con);

                cmd.Parameters.AddWithValue("@member_id", TextBox2.Text.Trim());
                cmd.Parameters.AddWithValue("@member_name", TextBox3.Text.Trim());
                cmd.Parameters.AddWithValue("@book_id", TextBox1.Text.Trim());
                cmd.Parameters.AddWithValue("@book_name", TextBox4.Text.Trim());
                cmd.Parameters.AddWithValue("@issue_date", TextBox5.Text.Trim());
                cmd.Parameters.AddWithValue("@due_date", TextBox6.Text.Trim());

                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("UPDATE book_master_tbl set current_stock=current_stock-1 WHERE book_id='" + TextBox1.Text.Trim() + "'AND current_stock>0'", con);
        
                cmd.ExecuteNonQuery();
                con.Close();
                Response.Write("<script>alert('Book issued Successfully');</script>");
              
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        bool checkifbookexists()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("SELECT * from book_master_tbl where book_id='" + TextBox1.Text.Trim() + "'AND current_stock>0", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //  Response.Write("<script>alert('" + ex.Message + "');</script>");
                return false;
            }
        }

        bool checkifmemberexists()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("SELECT full_name from member_master_tbl where member_id='" + TextBox2.Text.Trim() +"'", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception ex)
            {
                //  Response.Write("<script>alert('" + ex.Message + "');</script>");
                return false;
            }
        }
        bool checkifissueentryexists()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("SELECT * from book_issue_tbl where member_id='" + TextBox2.Text.Trim() + "'AND book_id='" + TextBox1.Text.Trim() + "'", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception ex)
            {
                //  Response.Write("<script>alert('" + ex.Message + "');</script>");
                return false;
            }
        }


        void getnames()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SELECT book_name from book_master_tbl where book_id='" + TextBox1.Text.Trim() + "'", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count >= 1)
                {
                    TextBox4.Text = dt.Rows[0]["book_name"].ToString();
                }
                else
                {
                    Response.Write("<script>alert('wrong book ID');</script>");
                }
                cmd = new SqlCommand("SELECT full_name from member_master_tbl where member_id='" + TextBox2.Text.Trim() + "'", con);
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count >= 1)
                {
                    TextBox3.Text = dt.Rows[0]["full_name"].ToString();
                }
                else
                {
                    Response.Write("<script>alert('wrong user ID');</script>");
                }
            }
            catch (Exception ex)
            {
                //  Response.Write("<script>alert('" + ex.Message + "');</script>");
                // return false;
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if(e.Row.RowType==DataControlRowType.DataRow)
                {
                    DateTime dt = Convert.ToDateTime(e.Row.Cells[5].Text);
                    DateTime today = DateTime.Today;
                    if(today>dt)
                    {
                        e.Row.BackColor = System.Drawing.Color.PaleVioletRed;
                    }


                }
            }
            catch(Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
    } }






