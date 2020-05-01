var Categories = []
function loadPurchaseId() {

    $.ajax({
        type: "GET",
        url: '/home/getProductCategories',
        success: function (data) {
            Categories = data;
            //render catagory
            renderCategory('#productCategory');
        }
    })

}

//fetch categories from database
function LoadCategory(element) {
    if (Categories.length == 0) {
        //ajax function for fetch data
        $.ajax({
            type: "GET",
            url: '/home/getProductCategories',
            success: function (data) {
                Categories = data;
                //render catagory
                renderCategory('#productCategory');
            }
        })
    }
    else {
        //render catagory to the element
        renderCategory(element);
    }
}

function renderCategory(element) {
    var $ele = $(element);
    $ele.empty();
    $ele.append($('<option/>').val('0').text('Select'));
    $.each(Categories, function (i, val) {
        $ele.append($('<option/>').val(val.CategoryID).text(val.CategortyName));
    })
}

//fetch products
function LoadProduct(categoryDD) {
    $.ajax({
        type: "GET",
        url: "/home/getProducts",
        data: { 'categoryID': $(categoryDD).val() },
        success: function (data) {
            //render products to appropriate dropdown
            renderProduct($(categoryDD).parents('.mycontainer').find('select.product'), data);
        },
        error: function (error) {
            console.log(error);
        }
    })
}

function renderProduct(element, data) {
    //render product
    var $ele = $(element);
    $ele.empty();
    $ele.append($('<option/>').val('0').text('Select'));
    $.each(data, function (i, val) {
        $ele.append($('<option/>').val(val.ProductID).text(val.ProductName));
    })
}

$(document).ready(function () {
    //Add button click event
    $('#add').click(function () {
        //validation and add order items
        var isAllValid = true;

        if (isAllValid) {
            var $newRow = $('#mainrow').clone().removeAttr('id');
            $('.pc', $newRow).val($('#taxselection1').val());
            $('.pc1', $newRow).val($('#taxes').val());


            //Replace add button with remove button
            $('#add', $newRow).addClass('remove').val('Remove').removeClass('btn-success').addClass('btn-danger');

            var ttl = $('#totalRate').val();
            var ttldcnt = $('#discount').val();
            if ($("#sngltaxes").is(":visible")) {
                var ttltx = $('#sngltaxes').val();
            } else {
                var ttltx = $('#taxes').val();
            }

            var ttlamt = $('#txableRate').val();

            //remove id attribute from new clone row
            $('#taxselection1,#dscrption,#pcs,#quantty,#rate1,#txableRate,#taxes,#disper,#discount,#totalRate', $newRow).removeAttr('id');
            $('span.error', $newRow).remove();
            //append clone row
            $('#orderdetailsItems').append($newRow);

            //clear select data
            $('#taxselection1,#taxes').val('0');

            $('#dscrption,#pcs,#quantty,#rate1,#txableRate,#disper,#discount,#totalRate').val('');
            $('#orderItemError').empty();
            if (isNaN(parseInt(document.getElementById("ttl").innerHTML))) {

                document.getElementById("ttl").innerHTML = parseFloat(ttl).toFixed(2);

            } else {

                document.getElementById("ttl").innerHTML = (parseFloat(document.getElementById("ttl").innerHTML) + parseFloat(ttl)).toFixed(2);
            }

            if (isNaN(parseInt(document.getElementById("ttldcnt").innerHTML))) {

                document.getElementById("ttldcnt").innerHTML = parseFloat(ttldcnt).toFixed(2);

            } else {

                document.getElementById("ttldcnt").innerHTML = (parseFloat(document.getElementById("ttldcnt").innerHTML) + parseFloat(ttldcnt)).toFixed(2);
            }

            if (isNaN(parseInt(document.getElementById("ttltx").innerHTML))) {

                document.getElementById("ttltx").innerHTML = parseFloat(ttltx).toFixed(2);

            } else {

                document.getElementById("ttltx").innerHTML = (parseFloat(document.getElementById("ttltx").innerHTML) + parseFloat(ttltx)).toFixed(2);
            }

            if (isNaN(parseInt(document.getElementById("ttlamt").innerHTML))) {

                document.getElementById("ttlamt").innerHTML = parseFloat(ttlamt).toFixed(2);

            } else {

                document.getElementById("ttlamt").innerHTML = (parseFloat(document.getElementById("ttlamt").innerHTML) + parseFloat(ttlamt)).toFixed(2);
            }

        }

    })

    //remove button click event
    $('#orderdetailsItems').on('click', '.remove', function () {

        var aa = $(this).parents('tr').children('td').children("input.rate").val();
        var dis = $(this).parents('tr').children('td').children("input.discount").val();
        if ($("#sngltaxes").is(":visible")) {
            var tx = $('#sngltaxes').val();
        } else {
            var tx = $(this).parents('tr').children('td').children("input.taxes").val();
        }

        var txrt = $(this).parents('tr').children('td').children("input.txableRate").val();


        $(this).parents('tr').remove();

        document.getElementById("ttl").innerHTML = (parseFloat(document.getElementById("ttl").innerHTML) - parseFloat(aa)).toFixed(2);
        document.getElementById("ttldcnt").innerHTML = (parseFloat(document.getElementById("ttldcnt").innerHTML) - parseFloat(dis)).toFixed(2);
        document.getElementById("ttltx").innerHTML = (parseFloat(document.getElementById("ttltx").innerHTML) - parseFloat(tx)).toFixed(2);
        document.getElementById("ttlamt").innerHTML = (parseFloat(document.getElementById("ttlamt").innerHTML) - parseFloat(txrt)).toFixed(2);

    });

    $('#submit').click(function () {
        var isAllValid = true;
       
        //validate order items
        $('#orderItemError').text('');
        var list = [];
        var stocklist = [];
        var errorItemCount = 0;
        $('#orderdetailsItems tbody tr').each(function (index, ele) {

            var orderItem = {
                Purcahse_number: ($('.purchaseNo').val()).toString(),
                Date: $('.orderDate').val(),
                Product_Token: ($('select.pc', this).val()).toString(),
                Pieces: parseInt($('.pcs', this).val()).toFixed(0),
                Quantity: parseFloat($('.quantty', this).val()).toFixed(3),
                Amount: parseFloat($('.rate1', this).val()).toFixed(2),
                Taxable_amount: parseFloat($('.txableRate', this).val()).toFixed(2),
                Tax: parseFloat($('.pc1', this).val()).toFixed(2),
                Discount_percent: parseFloat($('.disper', this).val()).toFixed(2),
                Discount: parseFloat($('.discount', this).val()).toFixed(2),
                Sub_Total: parseFloat($('.rate', this).val()).toFixed(2),
            }
            //var stockItem = {
            //    Purcahse_number: ($('.purchaseNo').val()).toString(),
            //    Date: $('.orderDate').val(),
            //    Product_Token: ($('select.pc', this).val()).toString(),
            //    Pieces: parseInt($('.pcs', this).val()).toFixed(0),
            //    Quantity: parseFloat($('.quantty', this).val()).toFixed(3),
            //    Tax: parseFloat($('.pc1', this).val()).toFixed(2),
            //    Sub_Total: parseFloat($('.rate', this).val()).toFixed(2),
            //}
            list.push(orderItem);
            //stocklist.push(stockItem);

        });
        if (list.length == 0) {
            $('#orderItemError').text('At least 1 order item required.');
            isAllValid = false;
        }
        //if (stocklist.length == 0) {
        //    $('#orderItemError').text('At least 1 order item required for stock.');
        //    isAllValid = false;
        //}
        if (isAllValid) {

            var data = {
                Purchase_Number: ($('.purchaseNo').val()).toString(),
                Date: $('.orderDate').val(),
                tax: parseFloat($('#sngltaxes').val()).toFixed(2),
                Dealer_Token_number: $('.dlr').val(),

                Total_discount: parseFloat(document.getElementById("ttldcnt").innerHTML).toFixed(2),
                Total_tax: parseFloat(document.getElementById("ttltx").innerHTML).toFixed(2),
                Rate_including_tax: parseFloat(document.getElementById("incldtx").innerHTML).toFixed(2),
                Total_amount: parseFloat(document.getElementById("ttl").innerHTML).toFixed(2),
                Purchase_details: list,
               // Stocks: stocklist
            }

            $(this).val('Please wait...');

            $.ajax({
                type: 'POST',
                url: '/Purchase/save',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status) {
                        alert('Successfully saved');
                        //here we will clear the form
                        list = [];
                        var a = null;
                        if ($('.chkrpt').is(":checked")) {
                           
                            window.open('http://localhost:8087/api/Purchase/GetPurchaseListReport?prdctno=' + ($('.purchaseNo').val()).toString(), 'name', 'width=600,height=400');
                             a = "1";
                        }
                        
                        if (a == "1") {
                            document.location.reload(true);
                        } else {
                            document.location.reload(true);
                        }
                        
                        //$('#orderNo,#orderDate,#description').val('');
                        //$('#orderdetailsItems').empty();
                    }
                    else {
                        alert('Something went in problem or duplicate number has been entered.');
                    }
                    $('#submit').val('Save');
                },
                error: function (error) {
                    console.log(error);
                    $('#submit').val('Save');
                }
            });
           
        }

    });

});

//LoadCategory($('#productCategory'));