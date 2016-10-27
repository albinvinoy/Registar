using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Threading;
using System.Configuration;

namespace St.Thomas_Registration
{
    public partial class New_Member : Form
    {
       ushort panel1_enter = 0;
        public New_Member()
        {
            InitializeComponent();
        }


        private void New_Member_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'churchDatabaseFileDataSet.PrimaryDetail' table. You can move, or remove it, as needed.
            this.primaryDetailTableAdapter.Fill(this.churchDatabaseFileDataSet.PrimaryDetail);
            WindowState = FormWindowState.Maximized;
            txtfamilyNumber.Text = getFamilyId().ToString();
            loadgridview();
        }

        // This will laod the grid view into the data adapter
        private void loadgridview()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.ChurchDatabaseFileConnectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = @"
SELECT MemberID, [Member Name], Relationship, DOB, [Place of Birth], FamilyDetailFK 
FROM dbo.PrimaryDetail
WHERE dbo.PrimaryDetail.FamilyDetailFK = @id;
";
                    command.Connection = connection;
                    try
                    {
                        connection.Open();
                        command.Parameters.Add("@Id", SqlDbType.Int).Value = txtfamilyNumber.Text;
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.SelectCommand = command;

                        DataTable Dt = new DataTable();
                        adapter.Fill(Dt);
                        primaryGridView.DataSource = Dt;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        private int getFamilyId()
        {
            int id = 0;
            using (SqlConnection connection = new SqlConnection(Globaldata.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    string sqlString = @"
                    SELECT MAX(Id)
                    FROM [dbo].[FamilyDetails]
                    ";
                    command.Connection = connection;
                    command.CommandText = sqlString;
                    command.CommandType = CommandType.Text;
                    try
                    {

                        id = Convert.ToInt32(command.ExecuteScalar());
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK);

                    }
                    finally
                    {
                        connection.Close();

                    }
                    return id + 1;
                }
            }
        }

        private int getPrimaryId()
        {
            // this will get the id for the primary details
            return 0;
        }
        #region "Boxes and DropDown"
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                txtDOBaptism.Enabled = false;
                txtBaptismName.Enabled = false;
                dateTimePicker2.Enabled = false;
                txtPlaceofBaptism.Enabled = false;
                txtGodParents.Enabled = false;
            }
            else
            {
                txtDOBaptism.Enabled = true;
                txtBaptismName.Enabled = true;
                dateTimePicker2.Enabled = true;
                txtPlaceofBaptism.Enabled = true;
                txtGodParents.Enabled = true;
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            txtDOJ.Text = dateTimePicker1.Value.ToShortDateString();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            txtDOBaptism.Text = dateTimePicker2.Value.ToShortDateString();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                txtDateofConfirmation.Enabled = false;
                txtPlaceofConfimation.Enabled = false;
                dateTimePicker3.Enabled = false;
            }
            else
            {
                txtDateofConfirmation.Enabled = true;
                txtPlaceofConfimation.Enabled = true;
                dateTimePicker3.Enabled = true;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                txtDateofCommunion.Enabled = false;
                dateTimePicker4.Enabled = false;
                txtPlaceofCommunion.Enabled = false;
            }
            else
            {
                txtDateofCommunion.Enabled = true;
                dateTimePicker4.Enabled = true;
                txtPlaceofCommunion.Enabled = true;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                txtDOM.Enabled = false;
                txtPlaceofMarriage.Enabled = false;
                dateTimePicker5.Enabled = false;
            }
            else
            {
                txtDOM.Enabled = true;
                txtPlaceofMarriage.Enabled = true;
                dateTimePicker5.Enabled = true;
            }
        }

        private void txtHOF_TextChanged(object sender, EventArgs e)
        {
            txttopname.Text = txtHOF.Text + " and ".ToString() + txtSpouse.Text;
        }

        private void txtSpouse_TextChanged(object sender, EventArgs e)
        {
            txttopname.Text = txtHOF.Text + " and ".ToString() + txtSpouse.Text;
        }

        private void txttopname_TextChanged(object sender, EventArgs e)
        {
        }

        private void dateTimePicker7_ValueChanged(object sender, EventArgs e)
        {
            txtDOB.Text = dateTimePicker7.Value.ToShortDateString();
        }
        #endregion
        // This is to enter the Family Details into the databse
        private void FillinFamilyDetails()
        {
            using (SqlConnection conn = new SqlConnection(Globaldata.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("FamilyDetailsProcedure", conn))
                {
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = getFamilyId();
                        cmd.Parameters.Add("@FamilyName", SqlDbType.VarChar).Value = txtFamilyName.Text;
                        cmd.Parameters.Add("@HOF", SqlDbType.VarChar).Value = txtHOF.Text;
                        cmd.Parameters.Add("@Spouse", SqlDbType.VarChar).Value = txtSpouse.Text;
                        cmd.Parameters.Add("@Saddr", SqlDbType.VarChar).Value = txtSteet.Text;
                        cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = txtCity.Text;
                        cmd.Parameters.Add("@State", SqlDbType.Char).Value = cmbState.Text;
                        cmd.Parameters.Add("@ZipCode", SqlDbType.Char).Value = txtzipcode.Text;
                        cmd.Parameters.Add("@WardName", SqlDbType.VarChar).Value = cmbWardName.Text;
                        cmd.Parameters.Add("@PhoneNumber", SqlDbType.Char).Value = txtHomePhone.Text;
                        cmd.Parameters.Add("@CellPhone", SqlDbType.Char).Value = txtCellNumber.Text;
                        cmd.Parameters.Add("@AltPhone", SqlDbType.Char).Value = txtPhone1.Text;
                        cmd.Parameters.Add("@Email1", SqlDbType.VarChar).Value = txtEmail1.Text;
                        cmd.Parameters.Add("@Email2", SqlDbType.VarChar).Value = txtEmail2.Text;
                        cmd.Parameters.Add("@IndiaParishName", SqlDbType.VarChar).Value = txtParishName.Text;
                        cmd.Parameters.Add("@IndiaParishAddress", SqlDbType.VarChar).Value = txtParishAdd.Text;
                        cmd.Parameters.Add("@Diocese", SqlDbType.VarChar).Value = txtDiocese.Text;
                        cmd.Parameters.Add("@MonthlyPledge", SqlDbType.VarChar).Value = txtMonthyPledge.Text;
                        cmd.Parameters.Add("@Notes", SqlDbType.VarChar).Value = txtNotes.Text;

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Error Writing into Database", MessageBoxButtons.OK);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        // This is to update the Family Details into the database
        private void UpdateFamilyDetails()
        {
            using (SqlConnection conn = new SqlConnection(Globaldata.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("FamilyDetailsProcedureUpdate", conn))
                {
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = getFamilyId();
                        cmd.Parameters.Add("@FamilyName", SqlDbType.VarChar).Value = txtFamilyName.Text;
                        cmd.Parameters.Add("@HOF", SqlDbType.VarChar).Value = txtHOF.Text;
                        cmd.Parameters.Add("@Spouse", SqlDbType.VarChar).Value = txtSpouse.Text;
                        cmd.Parameters.Add("@Saddr", SqlDbType.VarChar).Value = txtSteet.Text;
                        cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = txtCity.Text;
                        cmd.Parameters.Add("@State", SqlDbType.Char).Value = cmbState.Text;
                        cmd.Parameters.Add("@ZipCode", SqlDbType.Char).Value = txtzipcode.Text;
                        cmd.Parameters.Add("@WardName", SqlDbType.VarChar).Value = cmbWardName.Text;
                        cmd.Parameters.Add("@PhoneNumber", SqlDbType.Char).Value = txtHomePhone.Text;
                        cmd.Parameters.Add("@CellPhone", SqlDbType.Char).Value = txtCellNumber.Text;
                        cmd.Parameters.Add("@AltPhone", SqlDbType.Char).Value = txtPhone1.Text;
                        cmd.Parameters.Add("@Email1", SqlDbType.VarChar).Value = txtEmail1.Text;
                        cmd.Parameters.Add("@Email2", SqlDbType.VarChar).Value = txtEmail2.Text;
                        cmd.Parameters.Add("@IndiaParishName", SqlDbType.VarChar).Value = txtParishName.Text;
                        cmd.Parameters.Add("@IndiaParishAddress", SqlDbType.VarChar).Value = txtParishAdd.Text;
                        cmd.Parameters.Add("@Diocese", SqlDbType.VarChar).Value = txtDiocese.Text;
                        cmd.Parameters.Add("@MonthlyPledge", SqlDbType.VarChar).Value = txtMonthyPledge.Text;
                        cmd.Parameters.Add("@Notes", SqlDbType.VarChar).Value = txtNotes.Text;

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Error Writing into Database", MessageBoxButtons.OK);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        private void gridviewPrimary_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {

        }

        private void gridviewPrimary_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            txtDOB.Text = "";
            txtPlaceofBaptism.Text = "";
            txtDOJ.Text = "";
            txtDOBaptism.Text = "";
            txtPlaceofBaptism.Text = "";
            txtBaptismName.Text = "";
            txtGodParents.Text = "";
            txtDateofCommunion.Text = "";
            txtPlaceofConfimation.Text = "";
            txtDateofCommunion.Text = "";
            txtPlaceofCommunion.Text = "";
            txtDOM.Text = "";
            txtPlaceofMarriage.Text = "";
            txtDOJ.Text = "";
            txtReasonforLeaving.Text = "";
            txtNotes.Text = "";
            cmbStatus.SelectedIndex = -1;
            txtfamilyNumber.Text = "";
        }

        private void txtDOB_TextChanged(object sender, EventArgs e)
        {
            //int index = gridviewPrimary.CurrentCell.RowIndex;
            //gridviewPrimary.Rows[index].Cells[3].Value = txtDOB.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //int index = gridviewPrimary.CurrentCell.RowIndex;
            //gridviewPrimary.Rows[index].Cells[1].Value = textBox2.Text;
        }

        private void GridviewPrimary_GotFocus(object sender, System.EventArgs e)
        {
            MessageBox.Show("Got", "Message", MessageBoxButtons.OK);
        }

        private void GridviewPrimary_LostFocus(object sender, System.EventArgs e)
        {
            // Push data
        }

       

        private void fillByToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.primaryDetailTableAdapter.FillBy(this.churchDatabaseFileDataSet.PrimaryDetail);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void txtfamilyNumber_TextChanged(object sender, EventArgs e)
        {
            loadgridview();
        }

        private void primaryGridView_SelectionChanged(object sender, EventArgs e)
        {
            // MessageBox.Show("Leave", "Message", MessageBoxButtons.OK);
            var selectedRow = primaryGridView.CurrentCell.RowIndex;
            var ID = primaryGridView[0, selectedRow].Value;
            txtNameInfo.Text = ID.ToString();
            // MessageBox.Show(ID.ToString(), "Returned is", MessageBoxButtons.OK);
        }

        private void panel2_Leave(object sender, EventArgs e)
        {
            MessageBox.Show("Leaving Panel 2", "Message", MessageBoxButtons.OK);

        }

        private void panel2_Enter(object sender, EventArgs e)
        {
            MessageBox.Show("Enter Panel 2", "Message", MessageBoxButtons.OK);
        }

        private void panel1_Leave(object sender, EventArgs e)
        {
            //MessageBox.Show("Leave", "Message", MessageBoxButtons.OK);
            // Make sure to check that all the importantt txtboxes are filled in
            txtfamilyNumber.Text = getFamilyId().ToString();

            if (panel1_enter>0)
            {
               try
                {
                    UpdateFamilyDetails();
                }
                catch( Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
                }
            }
            else
            {

                if (txtfamilyNumber.Text == "")
                {
                    MessageBox.Show("ID cannot be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtFamilyName.Focus();
                }
                else if (txtSteet.Text == "")
                {
                    MessageBox.Show("Street Information Cannot be Empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSteet.Focus();
                }
                else if (txtCity.Text == "")
                {
                    MessageBox.Show("City Information Cannot be Empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCity.Focus();
                }
                else if (txtzipcode.Text == "")
                {
                    MessageBox.Show("Zip Code Information Cannot be Empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtzipcode.Focus();
                }
                else if (cmbWardName.Text == "")
                {
                    MessageBox.Show("Ward Information Cannot be Empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cmbWardName.Focus();
                }
                else if (txtHomePhone.Text == "")
                {
                    MessageBox.Show("Phone Information Cannot be Empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtHomePhone.Focus();
                }

                try
                {
                    FillinFamilyDetails();
                    panel1_enter++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void panel1_Enter(object sender, EventArgs e) // ******************
        {
            /*
            This is the main panel
            ISSUE MULTIPLE COPYIES INTO THE DATABASE AND THIS ISSUE NEEDS TO BE PROPERLY RESOLVED.
            */

            //if (txtFamilyName.Text != "")
            //{
            //    UpdateFamilyDetails();
            //}
            txtfamilyNumber.Text = getFamilyId().ToString();
        }

        private void primaryGridView_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show("Leaving primaryGridView Row Leave", "Testing", MessageBoxButtons.OK);

        }

        private void primaryGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


    }
}
