using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using OrdersProject.Models;
using System.Data;

namespace OrdersProject.Controllers
{
    public class OrdersController : ApiController
    {
        string connecton = System.Configuration.ConfigurationManager.ConnectionStrings["OrdersConnection"].ConnectionString;
        [HttpPost]
        public HttpResponseMessage CreateCustomer(Customer customer)
        {
            if (ModelState.IsValid)
            {
                SqlConnection conn = new SqlConnection(connecton);
                SqlCommand Cmd = new SqlCommand("usp_CreateCustomer", conn);
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("@Name", customer.Name);
                Cmd.Parameters.AddWithValue("@Email", customer.Email);
                Cmd.Parameters.AddWithValue("@Password", customer.Password);
                Cmd.Parameters.AddWithValue("@Address", customer.Address);
                Cmd.Parameters.AddWithValue("@Mobile", customer.MobileNo);
                conn.Open();
                // int i = 0;
                int i = Convert.ToInt32(Cmd.ExecuteScalar());
                if (i > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.Created, "Customer created");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Customer already exists");
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Enter the customer mandatory details");
            }
        }
        [HttpPost]
        public HttpResponseMessage CreateOrders(OrderedItems orderedItems)
        {
            if (ModelState.IsValid)
            {
                DataTable myTable = CreateTable();
                foreach (var item in orderedItems.itemsLists)
                {
                    myTable.Rows.Add(item.ItemName, item.Price);
                }
                SqlConnection conn = new SqlConnection(connecton);
                SqlCommand Cmd = new SqlCommand("usp_CreateOrders", conn);
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("@DeliveryAddress", orderedItems.DeliveryAddress);
                Cmd.Parameters.AddWithValue("@UserId", orderedItems.UserId);
                Cmd.Parameters.AddWithValue("@OrderedItem_Details", myTable);
                conn.Open();
                int i = 0;
                i = Convert.ToInt32(Cmd.ExecuteScalar());
                if (i > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.Created, "Your order was successfully delivered to given address");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Your order should be pending");
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Please give the address before you ordered something");
            }
        }
        static DataTable CreateTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ItemName", typeof(string));
            dt.Columns.Add("Price", typeof(Int32));
            return dt;
        }
        [HttpGet]
        public IHttpActionResult GetOrdersItemsList(int OrderId)
        {
            List<OrderedItems> lstOrders = new List<OrderedItems>();
            SqlConnection conn = new SqlConnection(connecton);
            SqlCommand Cmd = new SqlCommand("usp_GetOrders", conn);
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.AddWithValue("@OrderId", OrderId);
            conn.Open();
            using (SqlDataReader sdr = Cmd.ExecuteReader())
            {
                if (sdr.HasRows)
                {
                    
                    while (sdr.Read())
                    {
                        OrderedItems objOrderedItems = new OrderedItems();
                        objOrderedItems.OrderId = Convert.ToInt32(sdr["OrderedId"]);
                        objOrderedItems.ItemName = Convert.ToString(sdr["ItemName"]);
                        objOrderedItems.Price = Convert.ToInt32(sdr["Price"]);
                        objOrderedItems.DeliveryAddress = Convert.ToString(sdr["DeliveryAddress"]);
                        objOrderedItems.CreatedDate = Convert.ToString(sdr["CreatedDate"]);
                        objOrderedItems.UserId = Convert.ToInt32(sdr["fk_CustomerId"]);
                        lstOrders.Add(objOrderedItems);
                    }

                    return Json(lstOrders);
                }
                else
                {
                    return Json("No data found");
                }
            }


        }
    }
}
