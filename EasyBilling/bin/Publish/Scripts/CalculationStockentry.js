$('#employeeForm').on('change', '.purchprice, #rateperunit, .sgst, .cgst, #percent, .pcs, .total', function calculate() {

    var purchprice = $('#employeeForm').find('.purchprice').val();
    var rateperunit = $('#employeeForm').find('[name=rateperunit]:checked').val();
    var percent = $('#employeeForm').find('[name=percent]:checked').val();
    var sgst = $('#employeeForm').find('.sgst').val();
    var cgst = $('#employeeForm').find('.cgst').val();
    var pcs = $('#employeeForm').find('.pcs').val();
    var total = $('#employeeForm').find('.total').val();

   
    if (rateperunit == "2" && percent == "4") {
      
        total = (parseFloat(purchprice) + parseFloat(sgst * 1) + parseFloat(cgst * 1)).toFixed(2);
        
    }
    else if (rateperunit == "1" && percent == "4")
        total = ((parseFloat(purchprice) * parseInt(pcs)) + (parseFloat(sgst) + parseFloat(cgst))).toFixed(2);
    else if (rateperunit == "1" && percent == "3")
        total = ((parseFloat(purchprice) * parseInt(pcs)) + ((parseFloat(purchprice) * parseInt(pcs)) * (parseFloat(sgst) + parseFloat(cgst)) / 100)).toFixed(2);
    else if (rateperunit == "2" && percent == "3") {
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
        $('#employeeForm').find('.pcs').val(0);
            }
       

});
$('#selectionForm').on('change', '#isselection', function calculate() {

    var selectedId = $('#selectionForm').find('[name=isselection]:checked').val();
    if (selectedId == "itmtyre")
    {
        $('#selectionForm').find("#Item_tyre_Id").attr("placeholder", "Item tyre Id");
        $('#selectionForm').find('.compname').removeClass('hide');
        $('#selectionForm').find('.or').removeClass('hide');
    } else if (selectedId == "itmtube")
    {
        $('#selectionForm').find("#Item_tyre_Id").attr("placeholder", "Item tube Id");
        $('#selectionForm').find('.compname').removeClass('hide');
        $('#selectionForm').find('.or').removeClass('hide');
    }else
    {
        $('#selectionForm').find("#Item_tyre_Id").attr("placeholder", "Product Id");
        $('#selectionForm').find('.compname').addClass('hide');
        $('#selectionForm').find('.or').addClass('hide');
    }
});

$('#itmidTable').on('click', '.addstk', function calculate1() {
    //alert("dsuhghuguhgfrfg");
    //var tableToQuery = $("#itmidTable").DataTable();
    //alert(tableToQuery);
    //var selectedRow = $("#itmidTable tr.selected");
    //alert(selectedRow);
    //var rowdata = tableToQuery.row(selectedRow).data();
    //alert(rowdata);
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

