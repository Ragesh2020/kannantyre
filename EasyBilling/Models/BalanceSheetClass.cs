using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyBilling.Models
{
    public class BalanceSheetClass
    {
        public DateTime SDate { get; set; }
        public DateTime EDate { get; set; }
        public string CarryForwardDate { get; set; }
        public decimal CarryForward { get; set; }

        public string Purchase_Number { get; set; }
        public string Purchase_Details { get; set; }
        public string Purchase_Date { get; set; }
        public decimal Purchase_Total_amount { get; set; }
        public decimal Purchase_CGST { get; set; }
        public decimal Purchase_SGST { get; set; }
        public string Purch_BillBy { get; set; }

        public string Billing_Number { get; set; }
        public string Billing_Details { get; set; }
        public string Billing_Date { get; set; }
        public decimal Billing_Amount_paid { get; set; }
        public decimal Billing_Total_tax { get; set; }
        public decimal Billing_Balance { get; set; }
        public decimal Billing_Total { get; set; }
        public string Bill_BillBy { get; set; }

        public string Order_Number { get; set; }
        public string Order_Details { get; set; }
        public string Order_Date { get; set; }
        public decimal Order_Total_tax { get; set; }
        public decimal Order_Amount_paid { get; set; }
        public decimal Order_Balance { get; set; }
        public decimal Order_Total { get; set; }
        public string Order_BillBy { get; set; }

        public string ExpenseId { get; set; }
        public string Expense_Details { get; set; }
        public string ExenseDate { get; set; }
        public decimal ExpenseTotalGST{ get; set; }
        public decimal ExpenseTotal { get; set; }
        public decimal ExpenseBalance { get; set; }
        public string Expense_BillBy { get; set; }

        public decimal TotalincomeCGST { get; set; }
        public decimal TotalincomeSGST { get; set; }
        public decimal TotalExpensesCGST { get; set; }
        public decimal TotalExpensesSGST { get; set; }
        public decimal TotalIncmBalance { get; set; }
        public decimal TotalExpBalance { get; set; }
        public decimal TotalBalance { get; set; }
    }
}