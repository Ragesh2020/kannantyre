$('#employeeForm').on('change', '.purchprice, #rateperunit, .sgst, .cgst, #percent, .pcs, .total', function calculate() {

    var purchprice = parseFloat("0.00");
    //var rateperunit = parseFloat("0");
    //var percent = parseFloat("0");
    var sgst = parseFloat("0");
    var cgst = parseFloat("0");
    var pcs = parseFloat("0");
    var total = parseFloat("0");
    purchprice = $('#employeeForm').find('.purchprice').val();
    var rateperunit = $('#employeeForm').find('[name=rateperunit]:checked').val();
    var percent = $('#employeeForm').find('[name=percent]:checked').val();
     sgst = $('#employeeForm').find('.sgst').val();
     cgst = $('#employeeForm').find('.cgst').val();
     pcs = $('#employeeForm').find('.pcs').val();
     total = $('#employeeForm').find('.total').val();
 
     if ((rateperunit == "1" && percent == "3") || (rateperunit == "2" && percent == "3")) {
         if (sgst > 100)
             $('#employeeForm').find('.sgst').val(0.00);
         if (cgst > 100)
             $('#employeeForm').find('.cgst').val(0.00);
     }
     sgst = $('#employeeForm').find('.sgst').val();
     cgst = $('#employeeForm').find('.cgst').val();
         if (rateperunit == "2" && percent == "4") {
             $("#sgstlbl").text("(Rs.)");
             $("#cgstlbl").text("(Rs.)");
             total = (parseFloat(purchprice) + parseFloat(sgst * 1) + parseFloat(cgst * 1)).toFixed(2);

         }
         else if (rateperunit == "1" && percent == "4") {
             $("#sgstlbl").text("(Rs.)");
             $("#cgstlbl").text("(Rs.)");
             total = ((parseFloat(purchprice) * parseInt(pcs)) + (parseFloat(sgst) + parseFloat(cgst))).toFixed(2);
         }
         else if (rateperunit == "1" && percent == "3") {
             $("#sgstlbl").text("(%)");
             $("#cgstlbl").text("(%)");
             total = ((parseFloat(purchprice) * parseInt(pcs)) + ((parseFloat(purchprice) * parseInt(pcs)) * (parseFloat(sgst) + parseFloat(cgst)) / 100)).toFixed(2);
         }
         else if (rateperunit == "2" && percent == "3") {
             $("#sgstlbl").text("(%)");
             $("#cgstlbl").text("(%)");
             total = (parseFloat(purchprice) + (parseFloat(purchprice) * (parseFloat(sgst) + parseFloat(cgst)) / 100)).toFixed(2);

         }
         else {
             total = $('#employeeForm').find('.total').val(0.00);
         }
    
    if (parseFloat(total)>0) {
       
        //$('#employeeForm').find('.sgst').val(sgst);
        //$('#employeeForm').find('.cgst').val(cgst);
        //$('#employeeForm').find('.pcs').val(pcs.valueOf());
        $('#employeeForm').find('.total').val(total);
       // alert($('#employeeForm').find('.total').va(parseFloat(total)));

            }
    else {
       
        $('#employeeForm').find('.total').val(0.00);
       // $('#employeeForm').find('.pcs').val(0);
            }
       

});
$('#selectionForm').on('change', '#isselection', function calculate() {
    $('#editstk').find('.stk').val("");
    $('#editstk').find('.amt').val("");
    $('#editstk').find('.CGST').val("");
    $('#editstk').find('.SGST').val("");
    var selectedId = $('#selectionForm').find('[name=isselection]:checked').val();
    if (selectedId == "itmtyre")
    {
        //$('#selectionForm').find("#Item_tyre_Id").attr("placeholder", "Item tyre Id");
        $('#selectionForm').find('.itmid').removeClass('hide');
        $('#selectionForm').find('.itmtubeid').addClass('hide');
        $('#selectionForm').find('.proid').addClass('hide');
        $('#selectionForm').find('.proname').addClass('hide');
        $('#selectionForm').find('.compname').removeClass('hide');
        $('#selectionForm').find('.comptubename').addClass('hide');
        $('#selectionForm').find('.or').removeClass('hide');
    } else if (selectedId == "itmtube")
    {
        $('#selectionForm').find('.itmid').addClass('hide');
        $('#selectionForm').find('.itmtubeid').removeClass('hide');
        $('#selectionForm').find('.proid').addClass('hide');
        $('#selectionForm').find('.proname').addClass('hide');
        $('#selectionForm').find('.compname').addClass('hide');
        $('#selectionForm').find('.comptubename').removeClass('hide');
        $('#selectionForm').find('.or').removeClass('hide');
    }else
    {
       
        //$('#editstk').find('[value=11]').attr("checked", "checked");
        $('#selectionForm').find('.itmid').addClass('hide');
        $('#selectionForm').find('.itmtubeid').addClass('hide');
        $('#selectionForm').find('.proid').removeClass('hide');
        $('#selectionForm').find('.proname').removeClass('hide');
        $('#selectionForm').find('.compname').addClass('hide');
        $('#selectionForm').find('.comptubename').addClass('hide');
        $('#selectionForm').find('.or').addClass('hide');
    }
});

$('#itmidTable,#itmtubeidTable,#itmprodctidTable').on('click', '.addstk', function calculate1() {
    //alert("dsuhghuguhgfrfg");
    //var tableToQuery = $("#itmidTable").DataTable();
    //alert(tableToQuery);
    //var selectedRow = $("#itmidTable tr.selected");
    //alert(selectedRow);
    //var rowdata = tableToQuery.row(selectedRow).data();
    //alert(rowdata);
  //  $(':input', '#employeeForm').val('');//////////////////////////////meeee
 $('#employeeForm').find('.purchprice').val();
   
     $('#employeeForm').find('.sgst').val();
     $('#employeeForm').find('.cgst').val();
     $('#employeeForm').find('.pcs').val();
    $('#employeeForm').find('.total').val();
    $('#employeeForm').find('#Token_Number').val('');

    var dd = $(this).closest("tr").find('td:nth-child(1)').html();
    $('#employeeForm').find('.itmidbycomp').val(dd);
  
   
    //var assetID = ($(selectedRow).find("td:nth-child(2)").html());
    ////var assetID = rowdata.Item_Id;
    //alert(assetID);
    //var purchprice = $('#employeeForm').find('.purchprice').val();
    //var rateperunit = $('#employeeForm').find('[name=rateperunit]:checked').val();
    //var percent = $('#employeeForm').find('[name=percent]:checked').val();
    //var sgst = $('#employeeForm').find('.sgst').val();
    //var cgst = $('#employeeForm').find('.cgst').val();
    //var pcs = $('#employeeForm').find('.pcs').val();
    //var total = $('#employeeForm').find('.total').val();


});

$('#billchoosing').on('change', '.billdis,.billttl, .billpaid,.billbal,.billttlprimary,#percent', function calculate() {
   
    var discount = parseFloat("0");
    var paidamt = parseFloat("0");
    var balance = parseFloat("0");
    var total = parseFloat("0");
    var primarytotal = parseFloat("0");
    var amounttobepaid = parseFloat("0");
    discount = $('#billchoosing').find('.billdis').val();
  
    if ($('#selectionForm').find('.ttlamttobepaidsummry').text() != "") {
       
        amounttobepaid = $('#selectionForm').find('.ttlamttobepaidsummryduplct').val();
    }
    percent = $('#billchoosing').find('[name=percent]:checked').val();
    total = $('#billchoosing').find('.billttl').val();
    paidamt = $('#billchoosing').find('.billpaid').val();
    balance = $('#billchoosing').find('.billbal').val();
    primarytotal = $('#billchoosing').find('.billttlprimary').val();
    if (percent == "1" && discount != "" && total != "") {
        $('#billchoosing').find('.disperoramt').text("(%)");
        
        if (parseFloat(discount * 1) > 100) {
            alert("Discount percent cannot be more than 100");
            return false;
        } else {
            amounttobepaid = (parseFloat(primarytotal * 1) - (parseFloat(primarytotal * 1) * (parseFloat(discount * 1) / 100))).toFixed(2);
           // $('#billchoosing').find('.billttl').val(total);

        }
      
    } else if (percent == "2" && discount != "" && total != "") {
        $('#billchoosing').find('.disperoramt').text("(Rs.)");
        amounttobepaid = (parseFloat(primarytotal * 1) - parseFloat(discount * 1)).toFixed(2);
        // $('#billchoosing').find('.billttl').val(total);
     
    }

    if (paidamt != "") {
        if (parseFloat(paidamt) > parseFloat(amounttobepaid)) {
            alert("Paid amount should not more than amount to be paid.");
            $('#billchoosing').find('.billpaid').val(parseFloat("0"));
            $('#selectionForm').find('.ttlpaidsummry').text("Amount Paid: " + parseFloat("0"));
            return false;
        } else {
            balance = (parseFloat(amounttobepaid * 1) - parseFloat(paidamt * 1)).toFixed(2);
            $('#billchoosing').find('.billbal').val(balance);
        }
    } else {
        balance = (parseFloat(amounttobepaid * 1)).toFixed(2);
        $('#billchoosing').find('.billbal').val(balance);
    }
   
    //$('#selectionForm').find('.ttlitmsummry').val((parseFloat(total * 1)).toFixed(2));
    $('#selectionForm').find('.ttlamtsummry').text("Total Amount: " + total);
    if (percent == "2")
        $('#selectionForm').find('.ttldissummry').text("Total Discount: Rs. " + discount);
    else
        $('#selectionForm').find('.ttldissummry').text("Total Discount: " + (total * discount / 100).toFixed(2));
    if ($('[name=selectionForExecution]').val() == "Quotation") {
        $('#selectionForm').find('.ttlamttobepaidsummry').text("Total amount to be paid: " + total);
    } else {
        $('#selectionForm').find('.ttlamttobepaidsummry').text("Total amount to be paid: " + amounttobepaid);
    }
    //$('#selectionForm').find('.ttlamttobepaidsummry').text("Total amount to be paid: " + amounttobepaid);
  
    $('#selectionForm').find('.ttlpaidsummry').text("Amount Paid: " + paidamt);
    $('#selectionForm').find('.ttlbalsummry').text("Balance: " + balance);
});

$('#editstk').on('change', '.gstservice', function changeformat() {

    var selectedId = $('#selectionForm').find('[name=isselection]:checked').val();
    if (selectedId == "itmtyre") {
        //$('#selectionForm').find("#Item_tyre_Id").attr("placeholder", "Item tyre Id");
        $('#selectionForm').find('.itmid').removeClass('hide');
        $('#selectionForm').find('.itmtubeid').addClass('hide');
        $('#selectionForm').find('.proid').addClass('hide');
        $('#selectionForm').find('.proname').addClass('hide');
        $('#selectionForm').find('.compname').removeClass('hide');
        $('#selectionForm').find('.comptubename').addClass('hide');
        $('#selectionForm').find('.or').removeClass('hide');
    } else if (selectedId == "itmtube") {
        $('#selectionForm').find('.itmid').addClass('hide');
        $('#selectionForm').find('.itmtubeid').removeClass('hide');
        $('#selectionForm').find('.proid').addClass('hide');
        $('#selectionForm').find('.proname').addClass('hide');
        $('#selectionForm').find('.compname').addClass('hide');
        $('#selectionForm').find('.comptubename').removeClass('hide');
        $('#selectionForm').find('.or').removeClass('hide');
    } else {
        $('#selectionForm').find('.itmid').addClass('hide');
        $('#selectionForm').find('.itmtubeid').addClass('hide');
        $('#selectionForm').find('.proid').removeClass('hide');
        $('#selectionForm').find('.proname').removeClass('hide');
        $('#selectionForm').find('.compname').addClass('hide');
        $('#selectionForm').find('.comptubename').addClass('hide');
        $('#selectionForm').find('.or').addClass('hide');
    }
});