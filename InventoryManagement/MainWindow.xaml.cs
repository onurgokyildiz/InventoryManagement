using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using InventoryManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace InventoryManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            this.btnInsert.Click += async (a, e) => await btnInsert_Click(a, e);
            this.btnDelete.Click += async (a, e) => await btnDelete_Click(a, e);
            this.btnUpdate.Click += async (a, e) => await btnUpdate_Click(a, e);
            this.btnShowData.Click += async (a, e) => await btnShowData_Click(a, e);
        }
        public async Task btnInsert_Click(object sender, RoutedEventArgs e)
        {
            using (var ctx = new DBContext())
            {
                try
                {
                    var cmd = ctx.Database.GetDbConnection().CreateCommand();
                    cmd.Connection.Open();
                    cmd.CommandText = $"INSERT INTO [dbo].[InventoryManagement]" + $"(SerialNo,CategoryName,ProductName,ModelName,ConsumerName,EmployeeName,ConsumerAddress,SenderAddress,PurchaseDate,PriceTL,PriceDolar)" +
                                      $"VALUES " +
                                      $"('{txtSerialNo.Text.ToString()}', " +
                                      $"'{txtCategoryName.Text.ToString()}', " +
                                      $"'{txtProductName.Text.ToString()}', " +
                                      $"'{txtModelName.Text.ToString()}', " +
                                      $"'{txtConsumerName.Text.ToString()}', " +
                                      $"'{txtEmployeeName.Text.ToString()}', " +
                                      $"'{txtConsumerAddress.Text.ToString()}', " +
                                      $"'{txtSenderAddress.Text.ToString()}', " +
                                      $"'{DateTime.Today}', " +
                                      $"{decimal.Parse(txtPriceTL.Text)}," +
                                      $"{decimal.Parse(txtPriceDolar.Text)})";
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 6000;
                    await cmd.ExecuteNonQueryAsync();

                }
                catch (Exception ex)
                {   
                    throw ex;
                }
            }
        }
        public async Task btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var ctx = new DBContext())
                {
                    int selectedID = int.Parse(txtID.Text);
                    var cmd = ctx.Database.GetDbConnection().CreateCommand();
                    if (cmd.Connection.State == ConnectionState.Closed)
                    {
                        cmd.Connection.Open();
                    }
                    cmd.CommandText = $"UPDATE [dbo].[InventoryManagement]" +
                                      $"SET " +
                                      $"[SerialNo] ='{txtSerialNo.Text.ToString()}', " +
                                      $"[CategoryName] ='{txtCategoryName.Text.ToString()}', " +
                                      $"[ProductName] ='{txtProductName.Text.ToString()}', " +
                                      $"[ModelName] ='{txtModelName.Text.ToString()}', " +
                                      $"[ConsumerName] ='{txtConsumerName.Text.ToString()}', " +
                                      $"[EmployeeName] = '{txtEmployeeName.Text.ToString()}', " +
                                      $"[ConsumerAddress] ='{txtConsumerAddress.Text.ToString()}', " +
                                      $"[SenderAddress] ='{txtSenderAddress.Text.ToString()}', " +
                                      $"[PurchaseDate] ='{DateTime.Today}', " +
                                      $"[PriceTL] = {decimal.Parse(txtPriceTL.Text)}," +
                                      $"[PriceDolar] ={decimal.Parse(txtPriceDolar.Text)}" +
                                      $"WHERE ID={selectedID}";

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 6000;
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
         
        }
        public async Task btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var ctx = new DBContext())
                {
                    int selectedID = int.Parse(txtID.Text);
                    var cmd = ctx.Database.GetDbConnection().CreateCommand();
                    if (cmd.Connection.State == ConnectionState.Closed)
                    {
                        cmd.Connection.Open();
                    }
                    cmd.CommandText = $"DELETE FROM [dbo].[InventoryManagement] WHERE ID={selectedID}";
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 6000;
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
     
        }
        public async Task btnShowData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var ctx = new DBContext())
                {
                    var cmd = ctx.Database.GetDbConnection().CreateCommand();
                    if (cmd.Connection.State == ConnectionState.Closed)
                    {
                        cmd.Connection.Open();
                    }
                    cmd.CommandText = $"SELECT * FROM [dbo].[InventoryManagement]";
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 6000;
                    var reader = await cmd.ExecuteReaderAsync();

                    List<InventoryDbModel> dbModel = new List<InventoryDbModel>();

                    while (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            InventoryDbModel result = new InventoryDbModel();
                            result.ID = reader.GetFieldValue<int>("ID");
                            result.CategoryName = reader.GetFieldValue<string>("CategoryName");
                            result.ProductName = reader.GetFieldValue<string>("ProductName");
                            result.ModelName = reader.GetFieldValue<string>("ModelName");
                            result.ConsumerName = reader.GetFieldValue<string>("ConsumerName");
                            result.EmployeeName = reader.GetFieldValue<string>("EmployeeName");
                            result.ConsumerAddress = reader.GetFieldValue<string>("ConsumerAddress");
                            result.SenderAddress = reader.GetFieldValue<string>("SenderAddress");
                            result.PurchaseDate = reader.GetFieldValue<DateTime>("PurchaseDate");
                            result.PriceTL = reader.GetFieldValue<decimal>("PriceTL");
                            result.PriceDolar = reader.GetFieldValue<decimal>("PriceDolar");
                            dbModel.Add(result);
                        }
                        await reader.NextResultAsync();
                    }
                    viewData.ItemsSource = dbModel.ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
