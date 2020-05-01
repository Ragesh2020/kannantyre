$('#orderdetailsItems').on('change', '.pp', function calculate() {


    var pcs = $(this).parents('div.nwdv').find('.pcs').val();
    var quantty = $(this).parents('div.nwdv').find('.wt').val();
    var tax = $(this).parents('div.nwdv').find('.txrt').val();

    var hsncode = $(this).parents('div.nwdv').find('.hsn').val();
    var discount = $(this).parents('div.nwdv').find('.dscnt').val();
    var total = $(this).parents('div.nwdv').find('.ttl').val();
    var amount = $(this).parents('div.nwdv').find('.amt').val();
    var amountaftrtax = $(this).parents('div.nwdv').find('.amaftrtx').val();
    var stkin = $(this).parents('div.nwdv').find('.stkin').val();


    pcs = (pcs * 1).toFixed(0);
    quantty = (quantty * 1).toFixed(3);
    tax = (tax * 1).toFixed(2);
    discount = (discount * 1).toFixed(2);
    total = (total * 1).toFixed(2);
    amountaftrtax = (amountaftrtax * 1).toFixed(2);
    amount = (amount * 1).toFixed(2);

    if ($(this).parents('div.nwdv').find('.pcs').is(":visible")) {

        quantty = 0.000;
        stkin = (stkin * 1).toFixed(0);
    }
    else {

        pcs = 0;
        stkin = (stkin * 1).toFixed(3);
    }

    if (hsncode != null && hsncode != '') {

        if (pcs != null && pcs != '' && pcs != 0) {

            var ttl = (pcs * amount).toFixed(2);
            var ttl1 = 0.00;
            quantty = 0.000;
            var taxttl = ((pcs * amount) + ((ttl * tax) / 100)).toFixed(2);

            //if (discount != 0.00 || discount != '' || discount != null) {

            discount = (taxttl * discount) / 100;
            ttl1 = (taxttl - discount).toFixed(2);
            //}
            stkin = (stkin * 1).toFixed(0);

            if (parseInt(pcs.toString()) > parseInt(stkin.toString())) {
                alert("Piece must not be greater that Stock in piece");

                $(this).parents('div.nwdv').find('.pcs').val(0);
                $(this).parents('div.nwdv').find('.wt').val(0.000);

                $(this).parents('div.nwdv').find('.stkin').val(stkin);
                $(this).parents('div.nwdv').find('.dscnt').val(0.00);
                $(this).parents('div.nwdv').find('.ttl').val(0.00);
                $(this).parents('div.nwdv').find('.amt').val(0.00);
                $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
            }
            else {
                $(this).parents('div.nwdv').find('.pcs').val(pcs.valueOf());
                $(this).parents('div.nwdv').find('.wt').val(quantty.valueOf().toFixed(3));
                $(this).parents('div.nwdv').find('.txrt').val(tax.valueOf());
                $(this).parents('div.nwdv').find('.dscnt').val(discount.valueOf().toFixed(2));
                $(this).parents('div.nwdv').find('.ttl').val(ttl1.valueOf());
                $(this).parents('div.nwdv').find('.amt').val(amount.valueOf());

                $(this).parents('div.nwdv').find('.amaftrtx').val(taxttl.valueOf());
                // document.getElementById("dscnt").value = discount.toFixed();
            }
        }
        else if (quantty != null && quantty != '' && quantty != 0.000) {

            var ttl = (quantty * amount).toFixed(2);
            var ttl1 = 0.00;
            var taxttl = ((quantty * amount) + ((ttl * tax) / 100)).toFixed(2);

            discount = (taxttl * discount) / 100;
            ttl1 = (taxttl - discount).toFixed(2);

            stkin = (stkin * 1).toFixed(3);

            if (parseFloat(quantty.toString()) > parseFloat(stkin.toString())) {
                alert("Weight must not be greater that Stock in weight");

                $(this).parents('div.nwdv').find('.pcs').val(0);
                $(this).parents('div.nwdv').find('.wt').val(0.000);

                $(this).parents('div.nwdv').find('.stkin').val(stkin);
                $(this).parents('div.nwdv').find('.dscnt').val(0.00);
                $(this).parents('div.nwdv').find('.ttl').val(0.00);
                $(this).parents('div.nwdv').find('.amt').val(0.00);
                $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
            } else {

                $(this).parents('div.nwdv').find('.pcs').val(0);
                $(this).parents('div.nwdv').find('.wt').val(quantty);
                $(this).parents('div.nwdv').find('.txrt').val(tax);
                $(this).parents('div.nwdv').find('.stkin').val(stkin);
                $(this).parents('div.nwdv').find('.dscnt').val(discount);
                $(this).parents('div.nwdv').find('.ttl').val(ttl1);
                $(this).parents('div.nwdv').find('.amt').val(amount);
                $(this).parents('div.nwdv').find('.amaftrtx').val(taxttl);

            }
        }

        else {

            $(this).parents('div.nwdv').find('.pcs').val(0);
            $(this).parents('div.nwdv').find('.wt').val(0.000);


            $(this).parents('div.nwdv').find('.dscnt').val(0.00);
            $(this).parents('div.nwdv').find('.ttl').val(0.00);
            $(this).parents('div.nwdv').find('.amt').val(0.00);
            $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
        }
    } else {

        $(this).parents('div.nwdv').find('.pcs').val(0);
        $(this).parents('div.nwdv').find('.wt').val(0.000);
        $(this).parents('div.nwdv').find('.txrt').val(0.00);
        $(this).parents('div.nwdv').find('.stkin').val(0);
        $(this).parents('div.nwdv').find('.dscnt').val(0.00);
        $(this).parents('div.nwdv').find('.ttl').val(0.00);
        $(this).parents('div.nwdv').find('.amt').val(0.00);
        $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
    }

    //document.getElementById("pcs").value = pc.valueOf();
    //document.getElementById("wt").value = qnt.valueOf();
    //document.getElementById("tax").value = rt.valueOf();
    //document.getElementById("txableRate").value = ttl.valueOf();
    //document.getElementById("disper").value = dspr.valueOf();
    //document.getElementById("dscnt").value = ds.valueOf();
    //document.getElementById("totalRate").value = ttl1.valueOf();


});
$('#orderdetailsItems').on('change', '.txrt', function calculate() {


    var pcs = $(this).parents('div.nwdv').find('.pcs').val();
    var quantty = $(this).parents('div.nwdv').find('.wt').val();
    var tax = $(this).parents('div.nwdv').find('.txrt').val();

    var hsncode = $(this).parents('div.nwdv').find('.hsn').val();
    var discount = $(this).parents('div.nwdv').find('.dscnt').val();
    var total = $(this).parents('div.nwdv').find('.ttl').val();
    var amount = $(this).parents('div.nwdv').find('.amt').val();
    var amountaftrtax = $(this).parents('div.nwdv').find('.amaftrtx').val();
    var stkin = $(this).parents('div.nwdv').find('.stkin').val();


    pcs = (pcs * 1).toFixed(0);
    quantty = (quantty * 1).toFixed(3);
    tax = (tax * 1).toFixed(2);
    discount = (discount * 1).toFixed(2);
    total = (total * 1).toFixed(2);
    amountaftrtax = (amountaftrtax * 1).toFixed(2);
    amount = (amount * 1).toFixed(2);

    if ($(this).parents('div.nwdv').find('.pcs').is(":visible")) {

        quantty = 0.000;
        stkin = (stkin * 1).toFixed(0);
    }
    else {

        pcs = 0;
        stkin = (stkin * 1).toFixed(3);
    }

    if (hsncode != null && hsncode != '') {

        if (pcs != null && pcs != '' && pcs != 0) {

            var ttl = (pcs * amount).toFixed(2);
            var ttl1 = 0.00;
            quantty = 0.000;
            var taxttl = ((pcs * amount) + ((ttl * tax) / 100)).toFixed(2);

            //if (discount != 0.00 || discount != '' || discount != null) {

            discount = (taxttl * discount) / 100;
            ttl1 = (taxttl - discount).toFixed(2);
            //}
            stkin = (stkin * 1).toFixed(0);

            if (parseInt(pcs.toString()) > parseInt(stkin.toString())) {
                alert("Piece must not be greater that Stock in piece");

                $(this).parents('div.nwdv').find('.pcs').val(0);
                $(this).parents('div.nwdv').find('.wt').val(0.000);

                $(this).parents('div.nwdv').find('.stkin').val(stkin);
                $(this).parents('div.nwdv').find('.dscnt').val(0.00);
                $(this).parents('div.nwdv').find('.ttl').val(0.00);
                $(this).parents('div.nwdv').find('.amt').val(0.00);
                $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
            }
            else {
                $(this).parents('div.nwdv').find('.pcs').val(pcs.valueOf());
                $(this).parents('div.nwdv').find('.wt').val(quantty.valueOf().toFixed(3));
                $(this).parents('div.nwdv').find('.txrt').val(tax.valueOf());
                $(this).parents('div.nwdv').find('.dscnt').val(discount.valueOf().toFixed(2));
                $(this).parents('div.nwdv').find('.ttl').val(ttl1.valueOf());
                $(this).parents('div.nwdv').find('.amt').val(amount.valueOf());

                $(this).parents('div.nwdv').find('.amaftrtx').val(taxttl.valueOf());
                // document.getElementById("dscnt").value = discount.toFixed();
            }
        }
        else if (quantty != null && quantty != '' && quantty != 0.000) {

            var ttl = (quantty * amount).toFixed(2);
            var ttl1 = 0.00;
            var taxttl = ((quantty * amount) + ((ttl * tax) / 100)).toFixed(2);

            discount = (taxttl * discount) / 100;
            ttl1 = (taxttl - discount).toFixed(2);

            stkin = (stkin * 1).toFixed(3);

            if (parseFloat(quantty.toString()) > parseFloat(stkin.toString())) {
                alert("Weight must not be greater that Stock in weight");

                $(this).parents('div.nwdv').find('.pcs').val(0);
                $(this).parents('div.nwdv').find('.wt').val(0.000);

                $(this).parents('div.nwdv').find('.stkin').val(stkin);
                $(this).parents('div.nwdv').find('.dscnt').val(0.00);
                $(this).parents('div.nwdv').find('.ttl').val(0.00);
                $(this).parents('div.nwdv').find('.amt').val(0.00);
                $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
            } else {

                $(this).parents('div.nwdv').find('.pcs').val(0);
                $(this).parents('div.nwdv').find('.wt').val(quantty);
                $(this).parents('div.nwdv').find('.txrt').val(tax);
                $(this).parents('div.nwdv').find('.stkin').val(stkin);
                $(this).parents('div.nwdv').find('.dscnt').val(discount);
                $(this).parents('div.nwdv').find('.ttl').val(ttl1);
                $(this).parents('div.nwdv').find('.amt').val(amount);
                $(this).parents('div.nwdv').find('.amaftrtx').val(taxttl);

            }
        }

        else {

            $(this).parents('div.nwdv').find('.pcs').val(0);
            $(this).parents('div.nwdv').find('.wt').val(0.000);


            $(this).parents('div.nwdv').find('.dscnt').val(0.00);
            $(this).parents('div.nwdv').find('.ttl').val(0.00);
            $(this).parents('div.nwdv').find('.amt').val(0.00);
            $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
        }
    } else {

        $(this).parents('div.nwdv').find('.pcs').val(0);
        $(this).parents('div.nwdv').find('.wt').val(0.000);
        $(this).parents('div.nwdv').find('.txrt').val(0.00);
        $(this).parents('div.nwdv').find('.stkin').val(0);
        $(this).parents('div.nwdv').find('.dscnt').val(0.00);
        $(this).parents('div.nwdv').find('.ttl').val(0.00);
        $(this).parents('div.nwdv').find('.amt').val(0.00);
        $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
    }

    //document.getElementById("pcs").value = pc.valueOf();
    //document.getElementById("wt").value = qnt.valueOf();
    //document.getElementById("tax").value = rt.valueOf();
    //document.getElementById("txableRate").value = ttl.valueOf();
    //document.getElementById("disper").value = dspr.valueOf();
    //document.getElementById("dscnt").value = ds.valueOf();
    //document.getElementById("totalRate").value = ttl1.valueOf();


});
$('#orderdetailsItems').on('change', '.hsn', function calculate() {


    var pcs = $(this).parents('div.nwdv').find('.pcs').val();
    var quantty = $(this).parents('div.nwdv').find('.wt').val();
    var tax = $(this).parents('div.nwdv').find('.txrt').val();

    var hsncode = $(this).parents('div.nwdv').find('.hsn').val();
    var discount = $(this).parents('div.nwdv').find('.dscnt').val();
    var total = $(this).parents('div.nwdv').find('.ttl').val();
    var amount = $(this).parents('div.nwdv').find('.amt').val();
    var amountaftrtax = $(this).parents('div.nwdv').find('.amaftrtx').val();
    var stkin = $(this).parents('div.nwdv').find('.stkin').val();


    pcs = (pcs * 1).toFixed(0);
    quantty = (quantty * 1).toFixed(3);
    tax = (tax * 1).toFixed(2);
    discount = (discount * 1).toFixed(2);
    total = (total * 1).toFixed(2);
    amountaftrtax = (amountaftrtax * 1).toFixed(2);
    amount = (amount * 1).toFixed(2);

    if ($(this).parents('div.nwdv').find('.pcs').is(":visible")) {

        quantty = 0.000;
        stkin = (stkin * 1).toFixed(0);
    }
    else {

        pcs = 0;
        stkin = (stkin * 1).toFixed(3);
    }

    if (hsncode != null && hsncode != '') {

        if (pcs != null && pcs != '' && pcs != 0) {

            var ttl = (pcs * amount).toFixed(2);
            var ttl1 = 0.00;
            quantty = 0.000;
            var taxttl = ((pcs * amount) + ((ttl * tax) / 100)).toFixed(2);

            //if (discount != 0.00 || discount != '' || discount != null) {

            discount = (taxttl * discount) / 100;
            ttl1 = (taxttl - discount).toFixed(2);
            //}
            stkin = (stkin * 1).toFixed(0);

            if (parseInt(pcs.toString()) > parseInt(stkin.toString())) {
                alert("Piece must not be greater that Stock in piece");

                $(this).parents('div.nwdv').find('.pcs').val(0);
                $(this).parents('div.nwdv').find('.wt').val(0.000);

                $(this).parents('div.nwdv').find('.stkin').val(stkin);
                $(this).parents('div.nwdv').find('.dscnt').val(0.00);
                $(this).parents('div.nwdv').find('.ttl').val(0.00);
                $(this).parents('div.nwdv').find('.amt').val(0.00);
                $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
            }
            else {
                $(this).parents('div.nwdv').find('.pcs').val(pcs.valueOf());
                $(this).parents('div.nwdv').find('.wt').val(quantty.valueOf().toFixed(3));
                $(this).parents('div.nwdv').find('.txrt').val(tax.valueOf());
                $(this).parents('div.nwdv').find('.dscnt').val(discount.valueOf().toFixed(2));
                $(this).parents('div.nwdv').find('.ttl').val(ttl1.valueOf());
                $(this).parents('div.nwdv').find('.amt').val(amount.valueOf());

                $(this).parents('div.nwdv').find('.amaftrtx').val(taxttl.valueOf());
                // document.getElementById("dscnt").value = discount.toFixed();
            }
        }
        else if (quantty != null && quantty != '' && quantty != 0.000) {

            var ttl = (quantty * amount).toFixed(2);
            var ttl1 = 0.00;
            var taxttl = ((quantty * amount) + ((ttl * tax) / 100)).toFixed(2);

            discount = (taxttl * discount) / 100;
            ttl1 = (taxttl - discount).toFixed(2);

            stkin = (stkin * 1).toFixed(3);

            if (parseFloat(quantty.toString()) > parseFloat(stkin.toString())) {
                alert("Weight must not be greater that Stock in weight");

                $(this).parents('div.nwdv').find('.pcs').val(0);
                $(this).parents('div.nwdv').find('.wt').val(0.000);

                $(this).parents('div.nwdv').find('.stkin').val(stkin);
                $(this).parents('div.nwdv').find('.dscnt').val(0.00);
                $(this).parents('div.nwdv').find('.ttl').val(0.00);
                $(this).parents('div.nwdv').find('.amt').val(0.00);
                $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
            } else {

                $(this).parents('div.nwdv').find('.pcs').val(0);
                $(this).parents('div.nwdv').find('.wt').val(quantty);
                $(this).parents('div.nwdv').find('.txrt').val(tax);
                $(this).parents('div.nwdv').find('.stkin').val(stkin);
                $(this).parents('div.nwdv').find('.dscnt').val(discount);
                $(this).parents('div.nwdv').find('.ttl').val(ttl1);
                $(this).parents('div.nwdv').find('.amt').val(amount);
                $(this).parents('div.nwdv').find('.amaftrtx').val(taxttl);

            }
        }

        else {

            $(this).parents('div.nwdv').find('.pcs').val(0);
            $(this).parents('div.nwdv').find('.wt').val(0.000);


            $(this).parents('div.nwdv').find('.dscnt').val(0.00);
            $(this).parents('div.nwdv').find('.ttl').val(0.00);
            $(this).parents('div.nwdv').find('.amt').val(0.00);
            $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
        }
    } else {

        $(this).parents('div.nwdv').find('.pcs').val(0);
        $(this).parents('div.nwdv').find('.wt').val(0.000);
        $(this).parents('div.nwdv').find('.txrt').val(0.00);
        $(this).parents('div.nwdv').find('.stkin').val(0);
        $(this).parents('div.nwdv').find('.dscnt').val(0.00);
        $(this).parents('div.nwdv').find('.ttl').val(0.00);
        $(this).parents('div.nwdv').find('.amt').val(0.00);
        $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
    }

    //document.getElementById("pcs").value = pc.valueOf();
    //document.getElementById("wt").value = qnt.valueOf();
    //document.getElementById("tax").value = rt.valueOf();
    //document.getElementById("txableRate").value = ttl.valueOf();
    //document.getElementById("disper").value = dspr.valueOf();
    //document.getElementById("dscnt").value = ds.valueOf();
    //document.getElementById("totalRate").value = ttl1.valueOf();


});
$('#orderdetailsItems').on('change', '.stkin', function calculate() {


    var pcs = $(this).parents('div.nwdv').find('.pcs').val();
    var quantty = $(this).parents('div.nwdv').find('.wt').val();
    var tax = $(this).parents('div.nwdv').find('.txrt').val();

    var hsncode = $(this).parents('div.nwdv').find('.hsn').val();
    var discount = $(this).parents('div.nwdv').find('.dscnt').val();
    var total = $(this).parents('div.nwdv').find('.ttl').val();
    var amount = $(this).parents('div.nwdv').find('.amt').val();
    var amountaftrtax = $(this).parents('div.nwdv').find('.amaftrtx').val();
    var stkin = $(this).parents('div.nwdv').find('.stkin').val();


    pcs = (pcs * 1).toFixed(0);
    quantty = (quantty * 1).toFixed(3);
    tax = (tax * 1).toFixed(2);
    discount = (discount * 1).toFixed(2);
    total = (total * 1).toFixed(2);
    amountaftrtax = (amountaftrtax * 1).toFixed(2);
    amount = (amount * 1).toFixed(2);

    if ($(this).parents('div.nwdv').find('.pcs').is(":visible")) {

        quantty = 0.000;
        stkin = (stkin * 1).toFixed(0);
    }
    else {

        pcs = 0;
        stkin = (stkin * 1).toFixed(3);
    }

    if (hsncode != null && hsncode != '') {

        if (pcs != null && pcs != '' && pcs != 0) {

            var ttl = (pcs * amount).toFixed(2);
            var ttl1 = 0.00;
            quantty = 0.000;
            var taxttl = ((pcs * amount) + ((ttl * tax) / 100)).toFixed(2);

            //if (discount != 0.00 || discount != '' || discount != null) {

            discount = (taxttl * discount) / 100;
            ttl1 = (taxttl - discount).toFixed(2);
            //}
            stkin = (stkin * 1).toFixed(0);

            if (parseInt(pcs.toString()) > parseInt(stkin.toString())) {
                alert("Piece must not be greater that Stock in piece");

                $(this).parents('div.nwdv').find('.pcs').val(0);
                $(this).parents('div.nwdv').find('.wt').val(0.000);

                $(this).parents('div.nwdv').find('.stkin').val(stkin);
                $(this).parents('div.nwdv').find('.dscnt').val(0.00);
                $(this).parents('div.nwdv').find('.ttl').val(0.00);
                $(this).parents('div.nwdv').find('.amt').val(0.00);
                $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
            }
            else {
                $(this).parents('div.nwdv').find('.pcs').val(pcs.valueOf());
                $(this).parents('div.nwdv').find('.wt').val(quantty.valueOf().toFixed(3));
                $(this).parents('div.nwdv').find('.txrt').val(tax.valueOf());
                $(this).parents('div.nwdv').find('.dscnt').val(discount.valueOf().toFixed(2));
                $(this).parents('div.nwdv').find('.ttl').val(ttl1.valueOf());
                $(this).parents('div.nwdv').find('.amt').val(amount.valueOf());

                $(this).parents('div.nwdv').find('.amaftrtx').val(taxttl.valueOf());
                // document.getElementById("dscnt").value = discount.toFixed();
            }
        }
        else if (quantty != null && quantty != '' && quantty != 0.000) {

            var ttl = (quantty * amount).toFixed(2);
            var ttl1 = 0.00;
            var taxttl = ((quantty * amount) + ((ttl * tax) / 100)).toFixed(2);

            discount = (taxttl * discount) / 100;
            ttl1 = (taxttl - discount).toFixed(2);

            stkin = (stkin * 1).toFixed(3);

            if (parseFloat(quantty.toString()) > parseFloat(stkin.toString())) {
                alert("Weight must not be greater that Stock in weight");

                $(this).parents('div.nwdv').find('.pcs').val(0);
                $(this).parents('div.nwdv').find('.wt').val(0.000);

                $(this).parents('div.nwdv').find('.stkin').val(stkin);
                $(this).parents('div.nwdv').find('.dscnt').val(0.00);
                $(this).parents('div.nwdv').find('.ttl').val(0.00);
                $(this).parents('div.nwdv').find('.amt').val(0.00);
                $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
            } else {

                $(this).parents('div.nwdv').find('.pcs').val(0);
                $(this).parents('div.nwdv').find('.wt').val(quantty);
                $(this).parents('div.nwdv').find('.txrt').val(tax);
                $(this).parents('div.nwdv').find('.stkin').val(stkin);
                $(this).parents('div.nwdv').find('.dscnt').val(discount);
                $(this).parents('div.nwdv').find('.ttl').val(ttl1);
                $(this).parents('div.nwdv').find('.amt').val(amount);
                $(this).parents('div.nwdv').find('.amaftrtx').val(taxttl);

            }
        }

        else {

            $(this).parents('div.nwdv').find('.pcs').val(0);
            $(this).parents('div.nwdv').find('.wt').val(0.000);


            $(this).parents('div.nwdv').find('.dscnt').val(0.00);
            $(this).parents('div.nwdv').find('.ttl').val(0.00);
            $(this).parents('div.nwdv').find('.amt').val(0.00);
            $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
        }
    } else {

        $(this).parents('div.nwdv').find('.pcs').val(0);
        $(this).parents('div.nwdv').find('.wt').val(0.000);
        $(this).parents('div.nwdv').find('.txrt').val(0.00);
        $(this).parents('div.nwdv').find('.stkin').val(0);
        $(this).parents('div.nwdv').find('.dscnt').val(0.00);
        $(this).parents('div.nwdv').find('.ttl').val(0.00);
        $(this).parents('div.nwdv').find('.amt').val(0.00);
        $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
    }

    //document.getElementById("pcs").value = pc.valueOf();
    //document.getElementById("wt").value = qnt.valueOf();
    //document.getElementById("tax").value = rt.valueOf();
    //document.getElementById("txableRate").value = ttl.valueOf();
    //document.getElementById("disper").value = dspr.valueOf();
    //document.getElementById("dscnt").value = ds.valueOf();
    //document.getElementById("totalRate").value = ttl1.valueOf();


});
$('#orderdetailsItems').on('change', '.pcs', function calculate() {


    var pcs = $(this).parents('div.nwdv').find('.pcs').val();
    var quantty = $(this).parents('div.nwdv').find('.wt').val();
    var tax = $(this).parents('div.nwdv').find('.txrt').val();

    var hsncode = $(this).parents('div.nwdv').find('.hsn').val();
    var discount = $(this).parents('div.nwdv').find('.dscnt').val();
    var total = $(this).parents('div.nwdv').find('.ttl').val();
    var amount = $(this).parents('div.nwdv').find('.amt').val();
    var amountaftrtax = $(this).parents('div.nwdv').find('.amaftrtx').val();
    var stkin = $(this).parents('div.nwdv').find('.stkin').val();


    pcs = (pcs * 1).toFixed(0);
    quantty = (quantty * 1).toFixed(3);
    tax = (tax * 1).toFixed(2);
    discount = (discount * 1).toFixed(2);
    total = (total * 1).toFixed(2);
    amountaftrtax = (amountaftrtax * 1).toFixed(2);
    amount = (amount * 1).toFixed(2);

    if ($(this).parents('div.nwdv').find('.pcs').is(":visible")) {

        quantty = 0.000;
        stkin = (stkin * 1).toFixed(0);
    }
    else {

        pcs = 0;
        stkin = (stkin * 1).toFixed(3);
    }

    if (hsncode != null && hsncode != '') {

        if (pcs != null && pcs != '' && pcs != 0) {

            var ttl = (pcs * amount).toFixed(2);
            var ttl1 = 0.00;
            quantty = 0.000;
            var taxttl = ((pcs * amount) + ((ttl * tax) / 100)).toFixed(2);

            //if (discount != 0.00 || discount != '' || discount != null) {

            discount = (taxttl * discount) / 100;
            ttl1 = (taxttl - discount).toFixed(2);
            //}
            stkin = (stkin * 1).toFixed(0);

            if (parseInt(pcs.toString()) > parseInt(stkin.toString())) {
                alert("Piece must not be greater that Stock in piece");

                $(this).parents('div.nwdv').find('.pcs').val(0);
                $(this).parents('div.nwdv').find('.wt').val(0.000);

                $(this).parents('div.nwdv').find('.stkin').val(stkin);
                $(this).parents('div.nwdv').find('.dscnt').val(0.00);
                $(this).parents('div.nwdv').find('.ttl').val(0.00);
                $(this).parents('div.nwdv').find('.amt').val(0.00);
                $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
            }
            else {
                $(this).parents('div.nwdv').find('.pcs').val(pcs.valueOf());
                $(this).parents('div.nwdv').find('.wt').val(quantty.valueOf().toFixed(3));
                $(this).parents('div.nwdv').find('.txrt').val(tax.valueOf());
                $(this).parents('div.nwdv').find('.dscnt').val(discount.valueOf().toFixed(2));
                $(this).parents('div.nwdv').find('.ttl').val(ttl1.valueOf());
                $(this).parents('div.nwdv').find('.amt').val(amount.valueOf());

                $(this).parents('div.nwdv').find('.amaftrtx').val(taxttl.valueOf());
                // document.getElementById("dscnt").value = discount.toFixed();
            }
        }
        else if (quantty != null && quantty != '' && quantty != 0.000) {

            var ttl = (quantty * amount).toFixed(2);
            var ttl1 = 0.00;
            var taxttl = ((quantty * amount) + ((ttl * tax) / 100)).toFixed(2);

            discount = (taxttl * discount) / 100;
            ttl1 = (taxttl - discount).toFixed(2);

            stkin = (stkin * 1).toFixed(3);

            if (parseFloat(quantty.toString()) > parseFloat(stkin.toString())) {
                alert("Weight must not be greater that Stock in weight");

                $(this).parents('div.nwdv').find('.pcs').val(0);
                $(this).parents('div.nwdv').find('.wt').val(0.000);

                $(this).parents('div.nwdv').find('.stkin').val(stkin);
                $(this).parents('div.nwdv').find('.dscnt').val(0.00);
                $(this).parents('div.nwdv').find('.ttl').val(0.00);
                $(this).parents('div.nwdv').find('.amt').val(0.00);
                $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
            } else {

                $(this).parents('div.nwdv').find('.pcs').val(0);
                $(this).parents('div.nwdv').find('.wt').val(quantty);
                $(this).parents('div.nwdv').find('.txrt').val(tax);
                $(this).parents('div.nwdv').find('.stkin').val(stkin);
                $(this).parents('div.nwdv').find('.dscnt').val(discount);
                $(this).parents('div.nwdv').find('.ttl').val(ttl1);
                $(this).parents('div.nwdv').find('.amt').val(amount);
                $(this).parents('div.nwdv').find('.amaftrtx').val(taxttl);

            }
        }

        else {

            $(this).parents('div.nwdv').find('.pcs').val(0);
            $(this).parents('div.nwdv').find('.wt').val(0.000);


            $(this).parents('div.nwdv').find('.dscnt').val(0.00);
            $(this).parents('div.nwdv').find('.ttl').val(0.00);
            $(this).parents('div.nwdv').find('.amt').val(0.00);
            $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
        }
    } else {

        $(this).parents('div.nwdv').find('.pcs').val(0);
        $(this).parents('div.nwdv').find('.wt').val(0.000);
        $(this).parents('div.nwdv').find('.txrt').val(0.00);
        $(this).parents('div.nwdv').find('.stkin').val(0);
        $(this).parents('div.nwdv').find('.dscnt').val(0.00);
        $(this).parents('div.nwdv').find('.ttl').val(0.00);
        $(this).parents('div.nwdv').find('.amt').val(0.00);
        $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
    }

    //document.getElementById("pcs").value = pc.valueOf();
    //document.getElementById("wt").value = qnt.valueOf();
    //document.getElementById("tax").value = rt.valueOf();
    //document.getElementById("txableRate").value = ttl.valueOf();
    //document.getElementById("disper").value = dspr.valueOf();
    //document.getElementById("dscnt").value = ds.valueOf();
    //document.getElementById("totalRate").value = ttl1.valueOf();


});
$('#orderdetailsItems').on('change', '.wt', function calculate() {


    var pcs = $(this).parents('div.nwdv').find('.pcs').val();
    var quantty = $(this).parents('div.nwdv').find('.wt').val();
    var tax = $(this).parents('div.nwdv').find('.txrt').val();

    var hsncode = $(this).parents('div.nwdv').find('.hsn').val();
    var discount = $(this).parents('div.nwdv').find('.dscnt').val();
    var total = $(this).parents('div.nwdv').find('.ttl').val();
    var amount = $(this).parents('div.nwdv').find('.amt').val();
    var amountaftrtax = $(this).parents('div.nwdv').find('.amaftrtx').val();
    var stkin = $(this).parents('div.nwdv').find('.stkin').val();


    pcs = (pcs * 1).toFixed(0);
    quantty = (quantty * 1).toFixed(3);
    tax = (tax * 1).toFixed(2);
    discount = (discount * 1).toFixed(2);
    total = (total * 1).toFixed(2);
    amountaftrtax = (amountaftrtax * 1).toFixed(2);
    amount = (amount * 1).toFixed(2);

    if ($(this).parents('div.nwdv').find('.pcs').is(":visible")) {

        quantty = 0.000;
        stkin = (stkin * 1).toFixed(0);
    }
    else {

        pcs = 0;
        stkin = (stkin * 1).toFixed(3);
    }

    if (hsncode != null && hsncode != '') {

        if (pcs != null && pcs != '' && pcs != 0) {

            var ttl = (pcs * amount).toFixed(2);
            var ttl1 = 0.00;
            quantty = 0.000;
            var taxttl = ((pcs * amount) + ((ttl * tax) / 100)).toFixed(2);

            //if (discount != 0.00 || discount != '' || discount != null) {

            discount = (taxttl * discount) / 100;
            ttl1 = (taxttl - discount).toFixed(2);
            //}
            stkin = (stkin * 1).toFixed(0);

            if (parseInt(pcs.toString()) > parseInt(stkin.toString())) {
                alert("Piece must not be greater that Stock in piece");

                $(this).parents('div.nwdv').find('.pcs').val(0);
                $(this).parents('div.nwdv').find('.wt').val(0.000);

                $(this).parents('div.nwdv').find('.stkin').val(stkin);
                $(this).parents('div.nwdv').find('.dscnt').val(0.00);
                $(this).parents('div.nwdv').find('.ttl').val(0.00);
                $(this).parents('div.nwdv').find('.amt').val(0.00);
                $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
            }
            else {
                $(this).parents('div.nwdv').find('.pcs').val(pcs.valueOf());
                $(this).parents('div.nwdv').find('.wt').val(quantty.valueOf().toFixed(3));
                $(this).parents('div.nwdv').find('.txrt').val(tax.valueOf());
                $(this).parents('div.nwdv').find('.dscnt').val(discount.valueOf().toFixed(2));
                $(this).parents('div.nwdv').find('.ttl').val(ttl1.valueOf());
                $(this).parents('div.nwdv').find('.amt').val(amount.valueOf());

                $(this).parents('div.nwdv').find('.amaftrtx').val(taxttl.valueOf());
                // document.getElementById("dscnt").value = discount.toFixed();
            }
        }
        else if (quantty != null && quantty != '' && quantty != 0.000) {

            var ttl = (quantty * amount).toFixed(2);
            var ttl1 = 0.00;
            var taxttl = ((quantty * amount) + ((ttl * tax) / 100)).toFixed(2);

            discount = (taxttl * discount) / 100;
            ttl1 = (taxttl - discount).toFixed(2);

            stkin = (stkin * 1).toFixed(3);

            if (parseFloat(quantty.toString()) > parseFloat(stkin.toString())) {
                alert("Weight must not be greater that Stock in weight");

                $(this).parents('div.nwdv').find('.pcs').val(0);
                $(this).parents('div.nwdv').find('.wt').val(0.000);

                $(this).parents('div.nwdv').find('.stkin').val(stkin);
                $(this).parents('div.nwdv').find('.dscnt').val(0.00);
                $(this).parents('div.nwdv').find('.ttl').val(0.00);
                $(this).parents('div.nwdv').find('.amt').val(0.00);
                $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
            } else {

                $(this).parents('div.nwdv').find('.pcs').val(0);
                $(this).parents('div.nwdv').find('.wt').val(quantty);
                $(this).parents('div.nwdv').find('.txrt').val(tax);
                $(this).parents('div.nwdv').find('.stkin').val(stkin);
                $(this).parents('div.nwdv').find('.dscnt').val(discount);
                $(this).parents('div.nwdv').find('.ttl').val(ttl1);
                $(this).parents('div.nwdv').find('.amt').val(amount);
                $(this).parents('div.nwdv').find('.amaftrtx').val(taxttl);

            }
        }

        else {

            $(this).parents('div.nwdv').find('.pcs').val(0);
            $(this).parents('div.nwdv').find('.wt').val(0.000);


            $(this).parents('div.nwdv').find('.dscnt').val(0.00);
            $(this).parents('div.nwdv').find('.ttl').val(0.00);
            $(this).parents('div.nwdv').find('.amt').val(0.00);
            $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
        }
    } else {

        $(this).parents('div.nwdv').find('.pcs').val(0);
        $(this).parents('div.nwdv').find('.wt').val(0.000);
        $(this).parents('div.nwdv').find('.txrt').val(0.00);
        $(this).parents('div.nwdv').find('.stkin').val(0);
        $(this).parents('div.nwdv').find('.dscnt').val(0.00);
        $(this).parents('div.nwdv').find('.ttl').val(0.00);
        $(this).parents('div.nwdv').find('.amt').val(0.00);
        $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
    }

    //document.getElementById("pcs").value = pc.valueOf();
    //document.getElementById("wt").value = qnt.valueOf();
    //document.getElementById("tax").value = rt.valueOf();
    //document.getElementById("txableRate").value = ttl.valueOf();
    //document.getElementById("disper").value = dspr.valueOf();
    //document.getElementById("dscnt").value = ds.valueOf();
    //document.getElementById("totalRate").value = ttl1.valueOf();


});
$('#orderdetailsItems').on('change', '.amt', function calculate() {


    var pcs = $(this).parents('div.nwdv').find('.pcs').val();
    var quantty = $(this).parents('div.nwdv').find('.wt').val();
    var tax = $(this).parents('div.nwdv').find('.txrt').val();

    var hsncode = $(this).parents('div.nwdv').find('.hsn').val();
    var discount = $(this).parents('div.nwdv').find('.dscnt').val();
    var total = $(this).parents('div.nwdv').find('.ttl').val();
    var amount = $(this).parents('div.nwdv').find('.amt').val();
    var amountaftrtax = $(this).parents('div.nwdv').find('.amaftrtx').val();
    var stkin = $(this).parents('div.nwdv').find('.stkin').val();


    pcs = (pcs * 1).toFixed(0);
    quantty = (quantty * 1).toFixed(3);
    tax = (tax * 1).toFixed(2);
    discount = (discount * 1).toFixed(2);
    total = (total * 1).toFixed(2);
    amountaftrtax = (amountaftrtax * 1).toFixed(2);
    amount = (amount * 1).toFixed(2);

    if ($(this).parents('div.nwdv').find('.pcs').is(":visible")) {

        quantty = 0.000;
        stkin = (stkin * 1).toFixed(0);
    }
    else {

        pcs = 0;
        stkin = (stkin * 1).toFixed(3);
    }

    if (hsncode != null && hsncode != '') {

        if (pcs != null && pcs != '' && pcs != 0) {

            var ttl = (pcs * amount).toFixed(2);
            var ttl1 = 0.00;
            quantty = 0.000;
            var taxttl = ((pcs * amount) + ((ttl * tax) / 100)).toFixed(2);

            //if (discount != 0.00 || discount != '' || discount != null) {

            discount = (taxttl * discount) / 100;
            ttl1 = (taxttl - discount).toFixed(2);
            //}
            stkin = (stkin * 1).toFixed(0);

            if (parseInt(pcs.toString()) > parseInt(stkin.toString())) {
                alert("Piece must not be greater that Stock in piece");

                $(this).parents('div.nwdv').find('.pcs').val(0);
                $(this).parents('div.nwdv').find('.wt').val(0.000);

                $(this).parents('div.nwdv').find('.stkin').val(stkin);
                $(this).parents('div.nwdv').find('.dscnt').val(0.00);
                $(this).parents('div.nwdv').find('.ttl').val(0.00);
                $(this).parents('div.nwdv').find('.amt').val(0.00);
                $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
            }
            else {
                $(this).parents('div.nwdv').find('.pcs').val(pcs.valueOf());
                $(this).parents('div.nwdv').find('.wt').val(quantty.valueOf().toFixed(3));
                $(this).parents('div.nwdv').find('.txrt').val(tax.valueOf());
                $(this).parents('div.nwdv').find('.dscnt').val(discount.valueOf().toFixed(2));
                $(this).parents('div.nwdv').find('.ttl').val(ttl1.valueOf());
                $(this).parents('div.nwdv').find('.amt').val(amount.valueOf());

                $(this).parents('div.nwdv').find('.amaftrtx').val(taxttl.valueOf());
                // document.getElementById("dscnt").value = discount.toFixed();
            }
        }
        else if (quantty != null && quantty != '' && quantty != 0.000) {

            var ttl = (quantty * amount).toFixed(2);
            var ttl1 = 0.00;
            var taxttl = ((quantty * amount) + ((ttl * tax) / 100)).toFixed(2);

            discount = (taxttl * discount) / 100;
            ttl1 = (taxttl - discount).toFixed(2);

            stkin = (stkin * 1).toFixed(3);

            if (parseFloat(quantty.toString()) > parseFloat(stkin.toString())) {
                alert("Weight must not be greater that Stock in weight");

                $(this).parents('div.nwdv').find('.pcs').val(0);
                $(this).parents('div.nwdv').find('.wt').val(0.000);

                $(this).parents('div.nwdv').find('.stkin').val(stkin);
                $(this).parents('div.nwdv').find('.dscnt').val(0.00);
                $(this).parents('div.nwdv').find('.ttl').val(0.00);
                $(this).parents('div.nwdv').find('.amt').val(0.00);
                $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
            } else {

                $(this).parents('div.nwdv').find('.pcs').val(0);
                $(this).parents('div.nwdv').find('.wt').val(quantty);
                $(this).parents('div.nwdv').find('.txrt').val(tax);
                $(this).parents('div.nwdv').find('.stkin').val(stkin);
                $(this).parents('div.nwdv').find('.dscnt').val(discount);
                $(this).parents('div.nwdv').find('.ttl').val(ttl1);
                $(this).parents('div.nwdv').find('.amt').val(amount);
                $(this).parents('div.nwdv').find('.amaftrtx').val(taxttl);

            }
        }

        else {

            $(this).parents('div.nwdv').find('.pcs').val(0);
            $(this).parents('div.nwdv').find('.wt').val(0.000);


            $(this).parents('div.nwdv').find('.dscnt').val(0.00);
            $(this).parents('div.nwdv').find('.ttl').val(0.00);
            $(this).parents('div.nwdv').find('.amt').val(0.00);
            $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
        }
    } else {

        $(this).parents('div.nwdv').find('.pcs').val(0);
        $(this).parents('div.nwdv').find('.wt').val(0.000);
        $(this).parents('div.nwdv').find('.txrt').val(0.00);
        $(this).parents('div.nwdv').find('.stkin').val(0);
        $(this).parents('div.nwdv').find('.dscnt').val(0.00);
        $(this).parents('div.nwdv').find('.ttl').val(0.00);
        $(this).parents('div.nwdv').find('.amt').val(0.00);
        $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
    }

    //document.getElementById("pcs").value = pc.valueOf();
    //document.getElementById("wt").value = qnt.valueOf();
    //document.getElementById("tax").value = rt.valueOf();
    //document.getElementById("txableRate").value = ttl.valueOf();
    //document.getElementById("disper").value = dspr.valueOf();
    //document.getElementById("dscnt").value = ds.valueOf();
    //document.getElementById("totalRate").value = ttl1.valueOf();


});
$('#orderdetailsItems').on('change', '.amaftrtx', function calculate() {


    var pcs = $(this).parents('div.nwdv').find('.pcs').val();
    var quantty = $(this).parents('div.nwdv').find('.wt').val();
    var tax = $(this).parents('div.nwdv').find('.txrt').val();

    var hsncode = $(this).parents('div.nwdv').find('.hsn').val();
    var discount = $(this).parents('div.nwdv').find('.dscnt').val();
    var total = $(this).parents('div.nwdv').find('.ttl').val();
    var amount = $(this).parents('div.nwdv').find('.amt').val();
    var amountaftrtax = $(this).parents('div.nwdv').find('.amaftrtx').val();
    var stkin = $(this).parents('div.nwdv').find('.stkin').val();


    pcs = (pcs * 1).toFixed(0);
    quantty = (quantty * 1).toFixed(3);
    tax = (tax * 1).toFixed(2);
    discount = (discount * 1).toFixed(2);
    total = (total * 1).toFixed(2);
    amountaftrtax = (amountaftrtax * 1).toFixed(2);
    amount = (amount * 1).toFixed(2);

    if ($(this).parents('div.nwdv').find('.pcs').is(":visible")) {

        quantty = 0.000;
        stkin = (stkin * 1).toFixed(0);
    }
    else {

        pcs = 0;
        stkin = (stkin * 1).toFixed(3);
    }

    if (hsncode != null && hsncode != '') {

        if (pcs != null && pcs != '' && pcs != 0) {

            var ttl = (pcs * amount).toFixed(2);
            var ttl1 = 0.00;
            quantty = 0.000;
            var taxttl = ((pcs * amount) + ((ttl * tax) / 100)).toFixed(2);

            //if (discount != 0.00 || discount != '' || discount != null) {

            discount = (taxttl * discount) / 100;
            ttl1 = (taxttl - discount).toFixed(2);
            //}
            stkin = (stkin * 1).toFixed(0);

            if (parseInt(pcs.toString()) > parseInt(stkin.toString())) {
                alert("Piece must not be greater that Stock in piece");

                $(this).parents('div.nwdv').find('.pcs').val(0);
                $(this).parents('div.nwdv').find('.wt').val(0.000);

                $(this).parents('div.nwdv').find('.stkin').val(stkin);
                $(this).parents('div.nwdv').find('.dscnt').val(0.00);
                $(this).parents('div.nwdv').find('.ttl').val(0.00);
                $(this).parents('div.nwdv').find('.amt').val(0.00);
                $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
            }
            else {
                $(this).parents('div.nwdv').find('.pcs').val(pcs.valueOf());
                $(this).parents('div.nwdv').find('.wt').val(quantty.valueOf().toFixed(3));
                $(this).parents('div.nwdv').find('.txrt').val(tax.valueOf());
                $(this).parents('div.nwdv').find('.dscnt').val(discount.valueOf().toFixed(2));
                $(this).parents('div.nwdv').find('.ttl').val(ttl1.valueOf());
                $(this).parents('div.nwdv').find('.amt').val(amount.valueOf());

                $(this).parents('div.nwdv').find('.amaftrtx').val(taxttl.valueOf());
                // document.getElementById("dscnt").value = discount.toFixed();
            }
        }
        else if (quantty != null && quantty != '' && quantty != 0.000) {

            var ttl = (quantty * amount).toFixed(2);
            var ttl1 = 0.00;
            var taxttl = ((quantty * amount) + ((ttl * tax) / 100)).toFixed(2);

            discount = (taxttl * discount) / 100;
            ttl1 = (taxttl - discount).toFixed(2);

            stkin = (stkin * 1).toFixed(3);

            if (parseFloat(quantty.toString()) > parseFloat(stkin.toString())) {
                alert("Weight must not be greater that Stock in weight");

                $(this).parents('div.nwdv').find('.pcs').val(0);
                $(this).parents('div.nwdv').find('.wt').val(0.000);

                $(this).parents('div.nwdv').find('.stkin').val(stkin);
                $(this).parents('div.nwdv').find('.dscnt').val(0.00);
                $(this).parents('div.nwdv').find('.ttl').val(0.00);
                $(this).parents('div.nwdv').find('.amt').val(0.00);
                $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
            } else {

                $(this).parents('div.nwdv').find('.pcs').val(0);
                $(this).parents('div.nwdv').find('.wt').val(quantty);
                $(this).parents('div.nwdv').find('.txrt').val(tax);
                $(this).parents('div.nwdv').find('.stkin').val(stkin);
                $(this).parents('div.nwdv').find('.dscnt').val(discount);
                $(this).parents('div.nwdv').find('.ttl').val(ttl1);
                $(this).parents('div.nwdv').find('.amt').val(amount);
                $(this).parents('div.nwdv').find('.amaftrtx').val(taxttl);

            }
        }

        else {

            $(this).parents('div.nwdv').find('.pcs').val(0);
            $(this).parents('div.nwdv').find('.wt').val(0.000);


            $(this).parents('div.nwdv').find('.dscnt').val(0.00);
            $(this).parents('div.nwdv').find('.ttl').val(0.00);
            $(this).parents('div.nwdv').find('.amt').val(0.00);
            $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
        }
    } else {

        $(this).parents('div.nwdv').find('.pcs').val(0);
        $(this).parents('div.nwdv').find('.wt').val(0.000);
        $(this).parents('div.nwdv').find('.txrt').val(0.00);
        $(this).parents('div.nwdv').find('.stkin').val(0);
        $(this).parents('div.nwdv').find('.dscnt').val(0.00);
        $(this).parents('div.nwdv').find('.ttl').val(0.00);
        $(this).parents('div.nwdv').find('.amt').val(0.00);
        $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
    }

    //document.getElementById("pcs").value = pc.valueOf();
    //document.getElementById("wt").value = qnt.valueOf();
    //document.getElementById("tax").value = rt.valueOf();
    //document.getElementById("txableRate").value = ttl.valueOf();
    //document.getElementById("disper").value = dspr.valueOf();
    //document.getElementById("dscnt").value = ds.valueOf();
    //document.getElementById("totalRate").value = ttl1.valueOf();


});
$('#orderdetailsItems').on('change', '.dscnt', function calculate() {
    var pcs = $(this).parents('div.nwdv').find('.pcs').val();
    var quantty = $(this).parents('div.nwdv').find('.wt').val();
    var tax = $(this).parents('div.nwdv').find('.txrt').val();

    var hsncode = $(this).parents('div.nwdv').find('.hsn').val();
    var discount = $(this).parents('div.nwdv').find('.dscnt').val();
    var total = $(this).parents('div.nwdv').find('.ttl').val();
    var amount = $(this).parents('div.nwdv').find('.amt').val();
    var amountaftrtax = $(this).parents('div.nwdv').find('.amaftrtx').val();
    var stkin = $(this).parents('div.nwdv').find('.stkin').val();


    pcs = (pcs * 1).toFixed(0);
    quantty = (quantty * 1).toFixed(3);
    tax = (tax * 1).toFixed(2);
    discount = (discount * 1).toFixed(2);
    total = (total * 1).toFixed(2);
    amountaftrtax = (amountaftrtax * 1).toFixed(2);
    amount = (amount * 1).toFixed(2);

    if ($(this).parents('div.nwdv').find('.pcs').is(":visible")) {

        quantty = 0.000;
        stkin = (stkin * 1).toFixed(0);
    }
    else {

        pcs = 0;
        stkin = (stkin * 1).toFixed(3);
    }

    if (hsncode != null && hsncode != '') {

        if (pcs != null && pcs != '' && pcs != 0) {

            var ttl = (pcs * amount).toFixed(2);
            var ttl1 = 0.00;
            quantty = 0.000;
            var taxttl = ((pcs * amount) + ((ttl * tax) / 100)).toFixed(2);

            //if (discount != 0.00 || discount != '' || discount != null) {

            discount = (taxttl * discount) / 100;
            ttl1 = (taxttl - discount).toFixed(2);
            //}
            stkin = (stkin * 1).toFixed(0);

            if (parseInt(pcs.toString()) > parseInt(stkin.toString())) {
                alert("Piece must not be greater that Stock in piece");

                $(this).parents('div.nwdv').find('.pcs').val(0);
                $(this).parents('div.nwdv').find('.wt').val(0.000);

                $(this).parents('div.nwdv').find('.stkin').val(stkin);
                $(this).parents('div.nwdv').find('.dscnt').val(0.00);
                $(this).parents('div.nwdv').find('.ttl').val(0.00);
                $(this).parents('div.nwdv').find('.amt').val(0.00);
                $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
            }
            else {
                $(this).parents('div.nwdv').find('.pcs').val(pcs.valueOf());
                $(this).parents('div.nwdv').find('.wt').val(quantty.valueOf().toFixed(3));
                $(this).parents('div.nwdv').find('.txrt').val(tax.valueOf());
                $(this).parents('div.nwdv').find('.dscnt').val(discount.valueOf().toFixed(2));
                $(this).parents('div.nwdv').find('.ttl').val(ttl1.valueOf());
                $(this).parents('div.nwdv').find('.amt').val(amount.valueOf());

                $(this).parents('div.nwdv').find('.amaftrtx').val(taxttl.valueOf());
                // document.getElementById("dscnt").value = discount.toFixed();
            }
        }
        else if (quantty != null && quantty != '' && quantty != 0.000) {

            var ttl = (quantty * amount).toFixed(2);
            var ttl1 = 0.00;
            var taxttl = ((quantty * amount) + ((ttl * tax) / 100)).toFixed(2);

            discount = (taxttl * discount) / 100;
            ttl1 = (taxttl - discount).toFixed(2);

            stkin = (stkin * 1).toFixed(3);

            if (parseFloat(quantty.toString()) > parseFloat(stkin.toString())) {
                alert("Weight must not be greater that Stock in weight");

                $(this).parents('div.nwdv').find('.pcs').val(0);
                $(this).parents('div.nwdv').find('.wt').val(0.000);

                $(this).parents('div.nwdv').find('.stkin').val(stkin);
                $(this).parents('div.nwdv').find('.dscnt').val(0.00);
                $(this).parents('div.nwdv').find('.ttl').val(0.00);
                $(this).parents('div.nwdv').find('.amt').val(0.00);
                $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
            } else {

                $(this).parents('div.nwdv').find('.pcs').val(0);
                $(this).parents('div.nwdv').find('.wt').val(quantty);
                $(this).parents('div.nwdv').find('.txrt').val(tax);
                $(this).parents('div.nwdv').find('.stkin').val(stkin);
                $(this).parents('div.nwdv').find('.dscnt').val(discount);
                $(this).parents('div.nwdv').find('.ttl').val(ttl1);
                $(this).parents('div.nwdv').find('.amt').val(amount);
                $(this).parents('div.nwdv').find('.amaftrtx').val(taxttl);

            }
        }

        else {

            $(this).parents('div.nwdv').find('.pcs').val(0);
            $(this).parents('div.nwdv').find('.wt').val(0.000);


            $(this).parents('div.nwdv').find('.dscnt').val(0.00);
            $(this).parents('div.nwdv').find('.ttl').val(0.00);
            $(this).parents('div.nwdv').find('.amt').val(0.00);
            $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
        }
    } else {

        $(this).parents('div.nwdv').find('.pcs').val(0);
        $(this).parents('div.nwdv').find('.wt').val(0.000);
        $(this).parents('div.nwdv').find('.txrt').val(0.00);
        $(this).parents('div.nwdv').find('.stkin').val(0);
        $(this).parents('div.nwdv').find('.dscnt').val(0.00);
        $(this).parents('div.nwdv').find('.ttl').val(0.00);
        $(this).parents('div.nwdv').find('.amt').val(0.00);
        $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
    }

    //document.getElementById("pcs").value = pc.valueOf();
    //document.getElementById("wt").value = qnt.valueOf();
    //document.getElementById("tax").value = rt.valueOf();
    //document.getElementById("txableRate").value = ttl.valueOf();
    //document.getElementById("disper").value = dspr.valueOf();
    //document.getElementById("dscnt").value = ds.valueOf();
    //document.getElementById("totalRate").value = ttl1.valueOf();


});
$('#orderdetailsItems').on('change', '.ttl', function calculate() {
    var pcs = $(this).parents('div.nwdv').find('.pcs').val();
    var quantty = $(this).parents('div.nwdv').find('.wt').val();
    var tax = $(this).parents('div.nwdv').find('.txrt').val();

    var hsncode = $(this).parents('div.nwdv').find('.hsn').val();
    var discount = $(this).parents('div.nwdv').find('.dscnt').val();
    var total = $(this).parents('div.nwdv').find('.ttl').val();
    var amount = $(this).parents('div.nwdv').find('.amt').val();
    var amountaftrtax = $(this).parents('div.nwdv').find('.amaftrtx').val();
    var stkin = $(this).parents('div.nwdv').find('.stkin').val();


    pcs = (pcs * 1).toFixed(0);
    quantty = (quantty * 1).toFixed(3);
    tax = (tax * 1).toFixed(2);
    discount = (discount * 1).toFixed(2);
    total = (total * 1).toFixed(2);
    amountaftrtax = (amountaftrtax * 1).toFixed(2);
    amount = (amount * 1).toFixed(2);

    if ($(this).parents('div.nwdv').find('.pcs').is(":visible")) {

        quantty = 0.000;
        stkin = (stkin * 1).toFixed(0);
    }
    else {

        pcs = 0;
        stkin = (stkin * 1).toFixed(3);
    }

    if (hsncode != null && hsncode != '') {

        if (pcs != null && pcs != '' && pcs != 0) {

            var ttl = (pcs * amount).toFixed(2);
            var ttl1 = 0.00;
            quantty = 0.000;
            var taxttl = ((pcs * amount) + ((ttl * tax) / 100)).toFixed(2);

            //if (discount != 0.00 || discount != '' || discount != null) {

            discount = (taxttl * discount) / 100;
            ttl1 = (taxttl - discount).toFixed(2);
            //}
            stkin = (stkin * 1).toFixed(0);

            if (parseInt(pcs.toString()) > parseInt(stkin.toString())) {
                alert("Piece must not be greater that Stock in piece");

                $(this).parents('div.nwdv').find('.pcs').val(0);
                $(this).parents('div.nwdv').find('.wt').val(0.000);

                $(this).parents('div.nwdv').find('.stkin').val(stkin);
                $(this).parents('div.nwdv').find('.dscnt').val(0.00);
                $(this).parents('div.nwdv').find('.ttl').val(0.00);
                $(this).parents('div.nwdv').find('.amt').val(0.00);
                $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
            }
            else {
                $(this).parents('div.nwdv').find('.pcs').val(pcs.valueOf());
                $(this).parents('div.nwdv').find('.wt').val(quantty.valueOf().toFixed(3));
                $(this).parents('div.nwdv').find('.txrt').val(tax.valueOf());
                $(this).parents('div.nwdv').find('.dscnt').val(discount.valueOf().toFixed(2));
                $(this).parents('div.nwdv').find('.ttl').val(ttl1.valueOf());
                $(this).parents('div.nwdv').find('.amt').val(amount.valueOf());

                $(this).parents('div.nwdv').find('.amaftrtx').val(taxttl.valueOf());
                // document.getElementById("dscnt").value = discount.toFixed();
            }
        }
        else if (quantty != null && quantty != '' && quantty != 0.000) {

            var ttl = (quantty * amount).toFixed(2);
            var ttl1 = 0.00;
            var taxttl = ((quantty * amount) + ((ttl * tax) / 100)).toFixed(2);

            discount = (taxttl * discount) / 100;
            ttl1 = (taxttl - discount).toFixed(2);

            stkin = (stkin * 1).toFixed(3);

            if (parseFloat(quantty.toString()) > parseFloat(stkin.toString())) {
                alert("Weight must not be greater that Stock in weight");

                $(this).parents('div.nwdv').find('.pcs').val(0);
                $(this).parents('div.nwdv').find('.wt').val(0.000);

                $(this).parents('div.nwdv').find('.stkin').val(stkin);
                $(this).parents('div.nwdv').find('.dscnt').val(0.00);
                $(this).parents('div.nwdv').find('.ttl').val(0.00);
                $(this).parents('div.nwdv').find('.amt').val(0.00);
                $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
            } else {

                $(this).parents('div.nwdv').find('.pcs').val(0);
                $(this).parents('div.nwdv').find('.wt').val(quantty);
                $(this).parents('div.nwdv').find('.txrt').val(tax);
                $(this).parents('div.nwdv').find('.stkin').val(stkin);
                $(this).parents('div.nwdv').find('.dscnt').val(discount);
                $(this).parents('div.nwdv').find('.ttl').val(ttl1);
                $(this).parents('div.nwdv').find('.amt').val(amount);
                $(this).parents('div.nwdv').find('.amaftrtx').val(taxttl);

            }
        }

        else {

            $(this).parents('div.nwdv').find('.pcs').val(0);
            $(this).parents('div.nwdv').find('.wt').val(0.000);


            $(this).parents('div.nwdv').find('.dscnt').val(0.00);
            $(this).parents('div.nwdv').find('.ttl').val(0.00);
            $(this).parents('div.nwdv').find('.amt').val(0.00);
            $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
        }
    } else {

        $(this).parents('div.nwdv').find('.pcs').val(0);
        $(this).parents('div.nwdv').find('.wt').val(0.000);
        $(this).parents('div.nwdv').find('.txrt').val(0.00);
        $(this).parents('div.nwdv').find('.stkin').val(0);
        $(this).parents('div.nwdv').find('.dscnt').val(0.00);
        $(this).parents('div.nwdv').find('.ttl').val(0.00);
        $(this).parents('div.nwdv').find('.amt').val(0.00);
        $(this).parents('div.nwdv').find('.amaftrtx').val(0.00);
    }

    //document.getElementById("pcs").value = pc.valueOf();
    //document.getElementById("wt").value = qnt.valueOf();
    //document.getElementById("tax").value = rt.valueOf();
    //document.getElementById("txableRate").value = ttl.valueOf();
    //document.getElementById("disper").value = dspr.valueOf();
    //document.getElementById("dscnt").value = ds.valueOf();
    //document.getElementById("totalRate").value = ttl1.valueOf();


});
$('#orderdetailsItems').on('keypress', '.Subcategory', function autocomplete(e) {
   
    var Subcategory = String.fromCharCode(e.which);
    $('.Subcategory').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: 'http://localhost:8087/ProductsForSale/Getsubcat',
                dataType: "json",
                data: {
                    term: Subcategory

                },
                success: function (data) {
                    response(data);
                }
            });
        },
        //source: 'http://localhost:8087/ProductsForSale/Getsubcat',
        minLength: 1
    });
});
$('#mainrow').on('keypress', function autocomplete(e) {
    var Subcategory = String.fromCharCode(e.which);
   
    $('.Subcategory').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: 'http://localhost:8087/ProductsForSale/Getsubcat',
                dataType: "json",
                data: {
                    term: Subcategory
                    
                },
                success: function (data) {
                    response(data);
                }
            });
        },
        //source: 'http://localhost:8087/ProductsForSale/Getsubcat',
        minLength: 1
    });
});
$('#mainrow').on('change', function calculate() {
   

    var pcs = $(this).find('.pcs').val();
    var quantty = $(this).find('.wt').val();
    var tax = $(this).find('.txrt').val();
  
    var hsncode = $(this).find('.hsn').val();
    var discount = $(this).find('.dscnt').val();
    var total = $(this).find('.ttl').val();
    var amount = $(this).find('.amt').val();
    var amountaftrtax = $(this).find('.amaftrtx').val();
    var stkin = $(this).find('.stkin').val();


    pcs = (pcs * 1).toFixed(0);
    quantty = (quantty * 1).toFixed(3);
    tax = (tax * 1).toFixed(2);
    discount = (discount * 1).toFixed(2);
    total = (total * 1).toFixed(2);
    amountaftrtax = (amountaftrtax * 1).toFixed(2);
    amount = (amount * 1).toFixed(2);

    if ($('#mainrow').find('.pcs').is(":visible")) {

        quantty = 0.000;
        stkin = (stkin * 1).toFixed(0);
    }
    else {

        pcs = 0;
        stkin = (stkin * 1).toFixed(3);
    }
    if (hsncode != null && hsncode != '') {

        if (pcs != null && pcs != '' && pcs != 0) {

            var ttl = (pcs * amount).toFixed(2);
            var ttl1 = 0.00;
            quantty = 0.000;
            var taxttl = ((pcs * amount) + ((ttl * tax) / 100)).toFixed(2);

            //if (discount != 0.00 || discount != '' || discount != null) {

            discount = (taxttl * discount) / 100;
            ttl1 = (taxttl - discount).toFixed(2);
            //}
            stkin = (stkin * 1).toFixed(0);


            if (parseInt(pcs.toString()) > parseInt(stkin.toString())) {
                alert("Piece must not be greater that Stock in piece");

                $(this).find('.pcs').val(0);
                $(this).find('.wt').val(0.000);

                $(this).find('.stkin').val(stkin);
                $(this).find('.dscnt').val(0.00);
                $(this).find('.ttl').val(0.00);
                $(this).find('.amt').val(0.00);
                $(this).find('.amaftrtx').val(0.00);
            }
            else {
                $(this).find('.pcs').val(pcs.valueOf());
                $(this).find('.wt').val(quantty.valueOf().toFixed(3));
                $(this).find('.txrt').val(tax.valueOf());
                $(this).find('.dscnt').val(discount.valueOf().toFixed(2));
                $(this).find('.ttl').val(ttl1.valueOf());
                $(this).find('.amt').val(amount.valueOf());

                $(this).find('.amaftrtx').val(taxttl.valueOf());
                // document.getElementById("dscnt").value = discount.toFixed();
            }
        }
        else if (quantty != null && quantty != '' && quantty != 0.000) {

            var ttl = (quantty * amount).toFixed(2);
            var ttl1 = 0.00;
            var taxttl = ((quantty * amount) + ((ttl * tax) / 100)).toFixed(2);

            discount = (taxttl * discount) / 100;
            ttl1 = (taxttl - discount).toFixed(2);

            stkin = (stkin * 1).toFixed(3);

            if (parseFloat(quantty.toString()) > parseFloat(stkin.toString())) {
                alert("Weight must not be greater that Stock in weight");

                $('#mainrow').find('.pcs').val(0);
                $('#mainrow').find('.wt').val(0.000);

                $('#mainrow').find('.stkin').val(stkin);
                $('#mainrow').find('.dscnt').val(0.00);
                $('#mainrow').find('.ttl').val(0.00);
                $('#mainrow').find('.amt').val(0.00);
                $('#mainrow').find('.amaftrtx').val(0.00);
            } else {

                $('#mainrow').find('.pcs').val(0);
                $('#mainrow').find('.wt').val(quantty);
                $('#mainrow').find('.txrt').val(tax);
                $('#mainrow').find('.stkin').val(stkin);
                $('#mainrow').find('.dscnt').val(discount);
                $('#mainrow').find('.ttl').val(ttl1);
                $('#mainrow').find('.amt').val(amount);
                $('#mainrow').find('.amaftrtx').val(taxttl);

            }
        }

        else {

            $('#mainrow').find('.pcs').val(0);
            $('#mainrow').find('.wt').val(0.000);


            $('#mainrow').find('.dscnt').val(0.00);
            $('#mainrow').find('.ttl').val(0.00);
            $('#mainrow').find('.amt').val(0.00);
            $('#mainrow').find('.amaftrtx').val(0.00);
        }
    } else {

        $('#mainrow').find('.pcs').val(0);
        $('#mainrow').find('.wt').val(0.000);
        $('#mainrow').find('.txrt').val(0.00);
        $('#mainrow').find('.stkin').val(0);
        $('#mainrow').find('.dscnt').val(0.00);
        $('#mainrow').find('.ttl').val(0.00);
        $('#mainrow').find('.amt').val(0.00);
        $('#mainrow').find('.amaftrtx').val(0.00);
    }

    //document.getElementById("pcs").value = pc.valueOf();
    //document.getElementById("wt").value = qnt.valueOf();
    //document.getElementById("tax").value = rt.valueOf();
    //document.getElementById("txableRate").value = ttl.valueOf();
    //document.getElementById("disper").value = dspr.valueOf();
    //document.getElementById("dscnt").value = ds.valueOf();
    //document.getElementById("totalRate").value = ttl1.valueOf();



});
$('#ProceedModel').on('change', '.cih, .pbc, .pbch', function calpayment() {

    var cashinhand = $(this).parents('div#ProceedModel').find('.cih').val();
    var pybycrd = $(this).parents('div#ProceedModel').find('.pbc').val();
    var pybychk = $(this).parents('div#ProceedModel').find('.pbch').val();

    cashinhand = (cashinhand * 1).toFixed(2);
    pybycrd = (pybycrd * 1).toFixed(2);
    pybychk = (pybychk * 1).toFixed(2);
    var ttl = cashinhand + pybycrd + pybychk;
    var ttlamtppup = $('#mainrow').find('.amt').val();
    var ttltxppup = $('#mainrow').find('.amaftrtx').val() - $('#mainrow').find('.amt').val();
    var amtinctxppup = $('#mainrow').find('.amaftrtx').val();
    var ttldscntppup = $('#mainrow').find('.dscnt').val();
    var grndttlppup = $('#mainrow').find('.ttl').val();

    $('#orderdetailsItems div.nwdv').each(function (index, ele) {

        var i = index + 1;
        if ($('.remove' + i).find('.amt').val() != "") {
            ttlamtppup = (parseFloat(ttlamtppup) + parseFloat($('.remove' + i).find('.amt').val())).toFixed(2);
            ttltxppup = (parseFloat(ttltxppup) + parseFloat($('.remove' + i).find('.amaftrtx').val() - $('.remove' + i).find('.amt').val())).toFixed(2);
            amtinctxppup = (parseFloat(amtinctxppup) + parseFloat($('.remove' + i).find('.amaftrtx').val())).toFixed(2);
            ttldscntppup = (parseFloat(ttldscntppup) + parseFloat($('.remove' + i).find('.dscnt').val())).toFixed(2);
            grndttlppup = (parseFloat(grndttlppup) + parseFloat($('.remove' + i).find('.ttl').val())).toFixed(2);
        }

    });

    if (ttlamtppup == "" && grndttlppup == "") {
        $(this).parents('div#ProceedModel').find('.cih').val("");
        $(this).parents('div#ProceedModel').find('.pbc').val("");
        $(this).parents('div#ProceedModel').find('.pbch').val("");
        $("span.grndttlppup").text("0.00");
        alert("Atleast 1 product must be entered...");
        return false;
    } else {
        ttl = cashinhand + pybycrd + pybychk;
        if (parseFloat(grndttlppup) < parseFloat(ttl)) {
            alert("Amount must be equal or less than total amount...");
            $(this).parents('div#ProceedModel').find('.cih').val("");
            $(this).parents('div#ProceedModel').find('.pbc').val("");
            $(this).parents('div#ProceedModel').find('.pbch').val("");
        } else if (parseFloat(grndttlppup) > parseFloat($(this).parents('div#ProceedModel').find('.cih').val()) && $(this).parents('div#ProceedModel').find('.pbc').val() == "" && $(this).parents('div#ProceedModel').find('.pbch').val() == "") {

            var remain = (parseFloat(grndttlppup) - parseFloat($(this).parents('div#ProceedModel').find('.cih').val())).toFixed(2);
            $(this).parents('div#ProceedModel').find('.pbc').val(remain);
            $(this).parents('div#ProceedModel').find('.cih').val(cashinhand);
            $(this).parents('div#ProceedModel').find('.pbch').val(pybychk);
        } else if (parseFloat(grndttlppup) > parseFloat(ttl) && $(this).parents('div#ProceedModel').find('.pbc').val() != "" && $(this).parents('div#ProceedModel').find('.pbch').val() == "") {

            var remain1 = (parseFloat(grndttlppup) - (parseFloat($(this).parents('div#ProceedModel').find('.cih').val()) + parseFloat($(this).parents('div#ProceedModel').find('.pbc').val()))).toFixed(2);
            $(this).parents('div#ProceedModel').find('.pbch').val(remain1);
            $(this).parents('div#ProceedModel').find('.pbc').val(pybycrd);
            $(this).parents('div#ProceedModel').find('.cih').val(cashinhand);

        } else if (parseFloat(grndttlppup) > parseFloat($(this).parents('div#ProceedModel').find('.cih').val()) && $(this).parents('div#ProceedModel').find('.pbc').val() == "" && $(this).parents('div#ProceedModel').find('.pbch').val() != "") {

            var remain = (parseFloat(grndttlppup) - (parseFloat($(this).parents('div#ProceedModel').find('.cih').val()) + parseFloat($(this).parents('div#ProceedModel').find('.pbch').val()))).toFixed(2);
            $(this).parents('div#ProceedModel').find('.pbc').val(remain);

            $(this).parents('div#ProceedModel').find('.cih').val(cashinhand);
            $(this).parents('div#ProceedModel').find('.pbch').val(pybychk);
        }
        else if (parseFloat(grndttlppup) > parseFloat($(this).parents('div#ProceedModel').find('.cih').val()) && $(this).parents('div#ProceedModel').find('.pbc').val() != "" && $(this).parents('div#ProceedModel').find('.pbch').val() == "0.00") {

            var remain = (parseFloat(grndttlppup) - (parseFloat($(this).parents('div#ProceedModel').find('.cih').val()) + parseFloat($(this).parents('div#ProceedModel').find('.pbc').val()))).toFixed(2);
            $(this).parents('div#ProceedModel').find('.pbch').val(remain);

            $(this).parents('div#ProceedModel').find('.cih').val(cashinhand);
            $(this).parents('div#ProceedModel').find('.pbc').val(pybycrd);
        }
        else if (parseFloat(grndttlppup) > parseFloat(ttl) && $(this).parents('div#ProceedModel').find('.cih').val() == "" && $(this).parents('div#ProceedModel').find('.pbc').val() != "" && $(this).parents('div#ProceedModel').find('.pbch').val() == "0.00") {

            var remain = (parseFloat(grndttlppup) - (parseFloat($(this).parents('div#ProceedModel').find('.pbch').val()) + parseFloat($(this).parents('div#ProceedModel').find('.pbc').val()))).toFixed(2);
            $(this).parents('div#ProceedModel').find('.cih').val(remain);

            $(this).parents('div#ProceedModel').find('.pbch').val(pybychk);
            $(this).parents('div#ProceedModel').find('.pbc').val(pybycrd);
        }
        else {

        }
    }

});

$('#ProceedModel').on('click', '.prcd', function calpayment() {

    var cashinhand = $(this).parents('div#ProceedModel').find('.cih').val();
    var pybycrd = $(this).parents('div#ProceedModel').find('.pbc').val();
    var pybychk = $(this).parents('div#ProceedModel').find('.pbch').val();

    cashinhand = (cashinhand * 1).toFixed(2);
    pybycrd = (pybycrd * 1).toFixed(2);
    pybychk = (pybychk * 1).toFixed(2);
    var ttl = cashinhand + pybycrd + pybychk;
    var ttlamtppup = $('#mainrow').find('.amt').val();
    if ($(this).parents('div.nwdv').find('.pcs').is(":visible")) {
        var ttltxppup = $('#mainrow').find('.amaftrtx').val() - ($('#mainrow').find('.amt').val() * $('#mainrow').find('.pcs').val());
    } else {
        var ttltxppup = $('#mainrow').find('.amaftrtx').val() - ($('#mainrow').find('.amt').val() * $('#mainrow').find('.wt').val());
    }
    var amtinctxppup = $('#mainrow').find('.amaftrtx').val();
    var ttldscntppup = $('#mainrow').find('.dscnt').val();
    var grndttlppup = $('#mainrow').find('.ttl').val();

    $('#orderdetailsItems div.nwdv').each(function (index, ele) {

        var i = index + 1;
        if ($('.remove' + i).find('.amt').val() != "") {
            ttlamtppup = (parseFloat(ttlamtppup) + parseFloat($('.remove' + i).find('.amt').val())).toFixed(2);
            ttltxppup = (parseFloat(ttltxppup) + parseFloat($('.remove' + i).find('.amaftrtx').val() - $('.remove' + i).find('.amt').val())).toFixed(2);
            if ($(this).parents('div.nwdv').find('.pcs').is(":visible")) {
                ttltxppup = (parseFloat(ttltxppup) + parseFloat(($('.remove' + i).find('.amt').val() * $('.remove' + i).find('.pcs').val()))).toFixed(2);
            } else {

                ttltxppup = (parseFloat(ttltxppup) + parseFloat(($('.remove' + i).find('.amt').val() * $('.remove' + i).find('.wt').val()))).toFixed(2);
            }
            amtinctxppup = (parseFloat(amtinctxppup) + parseFloat($('.remove' + i).find('.amaftrtx').val())).toFixed(2);
            ttldscntppup = (parseFloat(ttldscntppup) + parseFloat($('.remove' + i).find('.dscnt').val())).toFixed(2);
            grndttlppup = (parseFloat(grndttlppup) + parseFloat($('.remove' + i).find('.ttl').val())).toFixed(2);
        }

    });

    if (ttlamtppup == "" && grndttlppup == "") {
        $(this).parents('div#ProceedModel').find('.cih').val("");
        $(this).parents('div#ProceedModel').find('.pbc').val("");
        $(this).parents('div#ProceedModel').find('.pbch').val("");
        $("span.grndttlppup").text("0.00");
        alert("Atleast 1 product must be entered...");
        return false;
    }
    else {
        ttl = cashinhand + pybycrd + pybychk;
        if (parseFloat(grndttlppup) < parseFloat(ttl)) {
            alert("Amount must be equal or less than total amount...");
            $(this).parents('div#ProceedModel').find('.cih').val("");
            $(this).parents('div#ProceedModel').find('.pbc').val("");
            $(this).parents('div#ProceedModel').find('.pbch').val("");
        } else if (parseFloat(grndttlppup) > parseFloat($(this).parents('div#ProceedModel').find('.cih').val()) && $(this).parents('div#ProceedModel').find('.pbc').val() == "" && $(this).parents('div#ProceedModel').find('.pbch').val() == "") {

            var remain = (parseFloat(grndttlppup) - parseFloat($(this).parents('div#ProceedModel').find('.cih').val())).toFixed(2);
            $(this).parents('div#ProceedModel').find('.pbc').val(remain);
            $(this).parents('div#ProceedModel').find('.cih').val(cashinhand);
            $(this).parents('div#ProceedModel').find('.pbch').val(pybychk);
        } else if (parseFloat(grndttlppup) > parseFloat(ttl) && $(this).parents('div#ProceedModel').find('.pbc').val() != "" && $(this).parents('div#ProceedModel').find('.pbch').val() == "") {

            var remain1 = (parseFloat(grndttlppup) - (parseFloat($(this).parents('div#ProceedModel').find('.cih').val()) + parseFloat($(this).parents('div#ProceedModel').find('.pbc').val()))).toFixed(2);
            $(this).parents('div#ProceedModel').find('.pbch').val(remain1);
            $(this).parents('div#ProceedModel').find('.pbc').val(pybycrd);
            $(this).parents('div#ProceedModel').find('.cih').val(cashinhand);

        } else if (parseFloat(grndttlppup) > parseFloat($(this).parents('div#ProceedModel').find('.cih').val()) && $(this).parents('div#ProceedModel').find('.pbc').val() == "" && $(this).parents('div#ProceedModel').find('.pbch').val() != "") {

            var remain = (parseFloat(grndttlppup) - (parseFloat($(this).parents('div#ProceedModel').find('.cih').val()) + parseFloat($(this).parents('div#ProceedModel').find('.pbch').val()))).toFixed(2);
            $(this).parents('div#ProceedModel').find('.pbc').val(remain);

            $(this).parents('div#ProceedModel').find('.cih').val(cashinhand);
            $(this).parents('div#ProceedModel').find('.pbch').val(pybychk);
        }
        else if (parseFloat(grndttlppup) > parseFloat($(this).parents('div#ProceedModel').find('.cih').val()) && $(this).parents('div#ProceedModel').find('.pbc').val() != "" && $(this).parents('div#ProceedModel').find('.pbch').val() == "0.00") {

            var remain = (parseFloat(grndttlppup) - (parseFloat($(this).parents('div#ProceedModel').find('.cih').val()) + parseFloat($(this).parents('div#ProceedModel').find('.pbc').val()))).toFixed(2);
            $(this).parents('div#ProceedModel').find('.pbch').val(remain);

            $(this).parents('div#ProceedModel').find('.cih').val(cashinhand);
            $(this).parents('div#ProceedModel').find('.pbc').val(pybycrd);
        }
        else if (parseFloat(grndttlppup) > parseFloat(ttl) && $(this).parents('div#ProceedModel').find('.cih').val() == "" && $(this).parents('div#ProceedModel').find('.pbc').val() != "" && $(this).parents('div#ProceedModel').find('.pbch').val() == "0.00") {

            var remain = (parseFloat(grndttlppup) - (parseFloat($(this).parents('div#ProceedModel').find('.pbch').val()) + parseFloat($(this).parents('div#ProceedModel').find('.pbc').val()))).toFixed(2);
            $(this).parents('div#ProceedModel').find('.cih').val(remain);

            $(this).parents('div#ProceedModel').find('.pbch').val(pybychk);
            $(this).parents('div#ProceedModel').find('.pbc').val(pybycrd);
        }
        else {

        }
    }

});

$('#submit1').click(function () {

    var isAllValid = true;

    //validate order items

    var list = [];
    //var stocklist = [];
    //var errorItemCount = 0;
    var orderItemMain = {
        Amount: parseFloat($('#mainrow').find('.amt').val()).toFixed(2),
        Amout_after_tax: parseFloat($('#mainrow').find('.amaftrtx').val()).toFixed(2),
        Discount: parseFloat($('#mainrow').find('.dscnt').val()).toFixed(2),
        Discount_last_date: $('#mainrow').find('.dt').val(),
        HSN_SAC_Code: $('#mainrow').find('.hsn').val(),
        Name: $('#mainrow').find('.nm').val(),
        Product_name: $('#mainrow').find('.pp').val(),
        Pieces: parseFloat($('#mainrow').find('.pcs').val()).toFixed(0),
        Sell_On: $('#mainrow').find('.sllon').val(),
        StockIn: $('#mainrow').find('.stkin').val(),
        Tax_rate: parseFloat($('#mainrow').find('.txrt').val()).toFixed(2),
        Total: parseFloat($('#mainrow').find('.ttl').val()).toFixed(2),
        Unit: $('#mainrow').find('.unt').val(),
        Weight: parseFloat($('#mainrow').find('.wt').val()).toFixed(3),
    }
    list.push(orderItemMain);

    $('#orderdetailsItems div.nwdv').each(function (index, ele) {
        var i = index + 1;
        if ($('.remove' + i).find('.amt').val() != "") {
            var orderItemnwdv = {
                Amount: parseFloat($('.remove' + i).find('.amt').val()).toFixed(2),
                Amout_after_tax: parseFloat($('.remove' + i).find('.amaftrtx').val()).toFixed(2),
                Discount: parseFloat($('.remove' + i).find('.dscnt').val()).toFixed(2),
                Discount_last_date: $('.remove' + i).find('.dt').val(),
                HSN_SAC_Code: $('.remove' + i).find('.hsn').val(),
                Name: $('.remove' + i).find('.nm').val(),
                Product_name: $('.remove' + i).find('.pp').val(),
                Pieces: parseFloat($('.remove' + i).find('.pcs').val()).toFixed(0),
                Sell_On: $('.remove' + i).find('.sllon').val(),
                StockIn: $('.remove' + i).find('.stkin').val(),
                Tax_rate: parseFloat($('.remove' + i).find('.txrt').val()).toFixed(2),
                Total: parseFloat($('.remove' + i).find('.ttl').val()).toFixed(2),
                Unit: $('.remove' + i).find('.unt').val(),
                Weight: parseFloat($('.remove' + i).find('.wt').val()).toFixed(3),
            }
        }
        list.push(orderItemnwdv);


    });

    if (isAllValid) {

        $(this).val('Please wait...');

        $.ajax({
            type: 'POST',
            url: 'http://localhost:8087/ProductsForSale/Create',
            data: JSON.stringify(list),
            contentType: 'application/json',
            success: function (data) {
                if (data.status) {
                    alert('Successfully saved');
                    //here we will clear the form
                    list = [];
                    var a = null;
                    alert($('.chkrpt').is(":checked"));
                    if ($('.chkrpt').is(":checked")) {

                        window.open('http://localhost:8087/api/Purchase/GetPurchaseListReport?prdctno=' + ($('.purchaseNo').val()).toString(), 'name', 'width=600,height=400');
                        a = "1";
                    }

                    if (a == "1") {
                        document.location.reload(true);
                    } else {
                        document.location.reload(true);
                    }

                }
                else {
                    alert(data.error);
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

$('#submit').click(function () {
   
    
    
    if ($('#myCheck').is(":checked")) {
        var isAllValid = true;

        //validate order items

        var list = [];
        //var stocklist = [];
        //var errorItemCount = 0;
        var fileUpload = $('#mainrow').find('.Image').get(0);
        var files = fileUpload.files;

        // Create FormData object  
        var fileData = new FormData();

        // Looping over all files and add it to FormData object  
        for (var i = 0; i < files.length; i++) {
            fileData.append(files[i].name, files[i]);
        }

        // Adding one more key to FormData object  
        fileData.append('username', 'Manas');

        var orderItemMain = {
            Amount: parseFloat($('#mainrow').find('.amt').val()).toFixed(2),
            Amout_after_tax: parseFloat($('#mainrow').find('.amaftrtx').val()).toFixed(2),
            Discount: parseFloat($('#mainrow').find('.dscnt').val()).toFixed(2),
            Discount_last_date: $('#mainrow').find('.dt').val(),
            HSN_SAC_Code: $('#mainrow').find('.hsn').val(),
            Name: $('#mainrow').find('.nm').val(),
            Product_name: $('#mainrow').find('.pp').val(),
            Pieces: parseFloat($('#mainrow').find('.pcs').val()).toFixed(0),
            Sell_On: $('#mainrow').find('.sllon').val(),
            StockIn: $('#mainrow').find('.stkin').val(),
            Tax_rate: parseFloat($('#mainrow').find('.txrt').val()).toFixed(2),
            Total: parseFloat($('#mainrow').find('.ttl').val()).toFixed(2),
            Unit: $('#mainrow').find('.unt').val(),
            Weight: parseFloat($('#mainrow').find('.wt').val()).toFixed(3),
            Subcategory: $('#mainrow').find('.Subcategory').val(),
            
            Image: fileData,
          
            Author_name: $('#mainrow').find('.Auname').val(),
            Publisher_name: $('#mainrow').find('.Puname').val(),
        }
        list.push(orderItemMain);
       
        $('#orderdetailsItems div.nwdv').each(function (index, ele) {

            var i = index + 1;
            if ($('.remove' + i).find('.amt').val() != "") {
                var fileUpload1 = $('.remove' + i).find('.Image').get(0);
                var files1 = fileUpload1.files;

                // Create FormData object  
                var fileData1 = new FormData();

                // Looping over all files and add it to FormData object  
                for (var i = 0; i < files1.length; i++) {
                    fileData1.append(files1[i].name, files1[i]);
                }

                // Adding one more key to FormData object  
                fileData1.append('username', 'Manas');
                var orderItemnwdv = {
                    Amount: parseFloat($('.remove' + i).find('.amt').val()).toFixed(2),
                    Amout_after_tax: parseFloat($('.remove' + i).find('.amaftrtx').val()).toFixed(2),
                    Discount: parseFloat($('.remove' + i).find('.dscnt').val()).toFixed(2),
                    Discount_last_date: $('.remove' + i).find('.dt').val(),
                    HSN_SAC_Code: $('.remove' + i).find('.hsn').val(),
                    Name: $('.remove' + i).find('.nm').val(),
                    Product_name: $('.remove' + i).find('.pp').val(),
                    Pieces: parseFloat($('.remove' + i).find('.pcs').val()).toFixed(0),
                    Sell_On: $('.remove' + i).find('.sllon').val(),
                    StockIn: $('.remove' + i).find('.stkin').val(),
                    Tax_rate: parseFloat($('.remove' + i).find('.txrt').val()).toFixed(2),
                    Total: parseFloat($('.remove' + i).find('.ttl').val()).toFixed(2),
                    Unit: $('.remove' + i).find('.unt').val(),
                    Weight: parseFloat($('.remove' + i).find('.wt').val()).toFixed(3),
                    Subcategory: $('.remove' + i).find('.Subcategory').val(),
                    Image: fileData1,
                   
                    Author_name: $('.remove' + i).find('.Auname').val(),
                    Publisher_name: $('.remove' + i).find('.Puname').val(),
                }
            }
            list.push(orderItemnwdv);


        });

        if (isAllValid) {

            $(this).val('Please wait...');

            $.ajax({
                type: 'POST',
                url: 'http://localhost:8087/ProductsForSale/Create',
                data: JSON.stringify({ purchase: null, products_For_SaleAsList: list }),
                contentType: 'application/json',
                //contentType: false, // Not to set any content header  
                processData: false, // Not to process data 
                success: function (data) {
                    if (data.status) {
                        alert('Successfully saved');
                        //here we will clear the form
                        list = [];
                        document.location.reload(true);
                    }
                    else {
                        alert(data.error);
                    }
                    $('#submit').val('Save');
                },
                error: function (error) {
                    console.log(error);
                    $('#submit').val('Save');
                }
            });

        }


    } else {

        if ($('#mainrow').find('.amt').val() == "") {
            var ttlamtppupfrprch = "0.00";
            var ttltxppupfrprch = "0.00";
            var amtinctxppupfrprch = "0.00";
            var ttldscntppupfrprch = "0.00";
            var grndttlppupfrprch = "0.00";
        }
        else {

            var ttlamtppupfrprch = $('#mainrow').find('.amt').val();
            var ttltxppupfrprch = $('#mainrow').find('.amaftrtx').val() - $('#mainrow').find('.amt').val();
            var amtinctxppupfrprch = $('#mainrow').find('.amaftrtx').val();
            var ttldscntppupfrprch = $('#mainrow').find('.dscnt').val();
            var grndttlppupfrprch = $('#mainrow').find('.ttl').val();
        }

        $('#orderdetailsItems div.nwdv').each(function (index, ele) {

            var i = index + 1;

            if ($('.remove' + i).find('.amt').val() != "") {
                ttlamtppupfrprch = (parseFloat(ttlamtppupfrprch) + parseFloat($('.remove' + i).find('.amt').val())).toFixed(2);
                ttltxppupfrprch = (parseFloat(ttltxppupfrprch) + parseFloat($('.remove' + i).find('.amaftrtx').val() - $('.remove' + i).find('.amt').val())).toFixed(2);
                amtinctxppupfrprch = (parseFloat(amtinctxppupfrprch) + parseFloat($('.remove' + i).find('.amaftrtx').val())).toFixed(2);
                ttldscntppupfrprch = (parseFloat(ttldscntppupfrprch) + parseFloat($('.remove' + i).find('.dscnt').val())).toFixed(2);
                grndttlppupfrprch = (parseFloat(grndttlppupfrprch) + parseFloat($('.remove' + i).find('.ttl').val())).toFixed(2);
            }

        });
        var isAllValid = true;
        var list = [];
        var orderlist = [];
        var ttltxppupmain = "0.00";
        if ($('#mainrow').find('.pcs').is(":visible")) {
            ttltxppupmain = (parseFloat(($('#mainrow').find('.amt').val() * $('#mainrow').find('.pcs').val()))).toFixed(2);
        }
        else {

            ttltxppupmain = (parseFloat(($('#mainrow').find('.amt').val() * $('#mainrow').find('.wt').val()))).toFixed(2);
        }
        var fileUpload = $('#mainrow').find('.Image').get(0);
        var files = fileUpload.files;

        // Create FormData object  
        var fileData = new FormData();

        // Looping over all files and add it to FormData object  
        for (var i = 0; i < files.length; i++) {
            fileData.append(files[i].name, files[i]);
        }

        // Adding one more key to FormData object  
        fileData.append('username', 'Manas');
        var orderItemMain = {
            Amount: parseFloat($('#mainrow').find('.amt').val()).toFixed(2),
            Amout_after_tax: parseFloat($('#mainrow').find('.amaftrtx').val()).toFixed(2),
            Discount: parseFloat($('#mainrow').find('.dscnt').val()).toFixed(2),
            Discount_last_date: $('#mainrow').find('.dt').val(),
            HSN_SAC_Code: $('#mainrow').find('.hsn').val(),
            Name: $('#mainrow').find('.nm').val(),
            Product_name: $('#mainrow').find('.pp').val(),
            Pieces: parseFloat($('#mainrow').find('.pcs').val()).toFixed(0),
            Sell_On: $('#mainrow').find('.sllon').val(),
            StockIn: $('#mainrow').find('.stkin').val(),
            Tax_rate: parseFloat($('#mainrow').find('.txrt').val()).toFixed(2),
            Total: parseFloat($('#mainrow').find('.ttl').val()).toFixed(2),
            Unit: $('#mainrow').find('.unt').val(),
            Weight: parseFloat($('#mainrow').find('.wt').val()).toFixed(3),
            Subcategory: $('#mainrow').find('.Subcategory').val(),
            Image: fileData,
            Author_name: $('#mainrow').find('.Auname').val(),
            Publisher_name: $('#mainrow').find('.Puname').val(),
        }
        var orderItemmainpurch = {
            Purcahse_number: $('.purchno').val(),

            Product_Token: $('#mainrow').find('.pp').val(),
            Product_name: $('#mainrow').find('.nm').val(),
            Pieces: parseFloat($('#mainrow').find('.pcs').val()).toFixed(0),
            Quantity: parseFloat($('#mainrow').find('.wt').val()).toFixed(3),
            Amount: parseFloat($('#mainrow').find('.amt').val()).toFixed(2),

            Tax: parseFloat($('#mainrow').find('.txrt').val()).toFixed(2),

            Taxable_amount: ttltxppupmain,

            Discount: parseFloat($('#mainrow').find('.dscnt').val()).toFixed(2),
            Sub_Total: parseFloat($('#mainrow').find('.ttl').val()).toFixed(2),
        }
        list.push(orderItemMain);
        orderlist.push(orderItemmainpurch);

        $('#orderdetailsItems div.nwdv').each(function (index, ele) {
            var i = index + 1;
            var ttltxppup = "0.00";
            if ($('.remove' + i).find('.amt').val() != "") {
                var fileUpload1 = $('.remove' + i).find('.Image').get(0);
                var files1 = fileUpload1.files;

                // Create FormData object  
                var fileData1 = new FormData();

                // Looping over all files and add it to FormData object  
                for (var i = 0; i < files1.length; i++) {
                    fileData1.append(files1[i].name, files1[i]);
                }

                // Adding one more key to FormData object  
                fileData1.append('username', 'Manas');
                var orderItemnwdv = {
                    Amount: parseFloat($('.remove' + i).find('.amt').val()).toFixed(2),
                    Amout_after_tax: parseFloat($('.remove' + i).find('.amaftrtx').val()).toFixed(2),
                    Discount: parseFloat($('.remove' + i).find('.dscnt').val()).toFixed(2),
                    Discount_last_date: $('.remove' + i).find('.dt').val(),
                    HSN_SAC_Code: $('.remove' + i).find('.hsn').val(),
                    Name: $('.remove' + i).find('.nm').val(),
                    Product_name: $('.remove' + i).find('.pp').val(),
                    Pieces: parseFloat($('.remove' + i).find('.pcs').val()).toFixed(0),
                    Sell_On: $('.remove' + i).find('.sllon').val(),
                    StockIn: $('.remove' + i).find('.stkin').val(),
                    Tax_rate: parseFloat($('.remove' + i).find('.txrt').val()).toFixed(2),
                    Total: parseFloat($('.remove' + i).find('.ttl').val()).toFixed(2),
                    Unit: $('.remove' + i).find('.unt').val(),
                    Weight: parseFloat($('.remove' + i).find('.wt').val()).toFixed(3),
                    Subcategory: $('.remove' + i).find('.Subcategory').val(),
                    Image: fileData1,
                    Author_name: $('.remove' + i).find('.Auname').val(),
                    Publisher_name: $('.remove' + i).find('.Puname').val(),
                }
                if ($('.remove' + i).find('.pcs').is(":visible")) {
                    ttltxppup = (parseFloat(ttltxppup) + parseFloat(($('.remove' + i).find('.amt').val() * $('.remove' + i).find('.pcs').val()))).toFixed(2);
                } else {

                    ttltxppup = (parseFloat(ttltxppup) + parseFloat(($('.remove' + i).find('.amt').val() * $('.remove' + i).find('.wt').val()))).toFixed(2);
                }
                var orderItempurch = {
                    Purcahse_number: $('.purchno').val(),

                    Product_Token: $('.remove' + i).find('.pp').val(),
                    Product_name: $('.remove' + i).find('.nm').val(),
                    Pieces: parseFloat($('.remove' + i).find('.pcs').val()).toFixed(0),
                    Quantity: parseFloat($('.remove' + i).find('.wt').val()).toFixed(3),
                    Amount: parseFloat($('.remove' + i).find('.amt').val()).toFixed(2),

                    Tax: parseFloat($('.remove' + i).find('.txrt').val()).toFixed(2),

                    Taxable_amount: ttltxppup,

                    Discount: parseFloat($('.remove' + i).find('.dscnt').val()).toFixed(2),
                    Sub_Total: parseFloat($('.remove' + i).find('.ttl').val()).toFixed(2),
                }
            }
            list.push(orderItemnwdv);
            orderlist.push(orderItempurch);

        });

        if (isAllValid) {


            var data = {
                Purchase_Number: $('.purchno').val(),

                tax: ttltxppupfrprch,
                Dealer_Token_number: $('.dlr').val(),

                Total_discount: ttldscntppupfrprch,
                Total_tax: ttltxppupfrprch,
                Rate_including_tax: amtinctxppupfrprch,
                Total_amount: grndttlppupfrprch,
                Purchase_details: orderlist,
            }

            $(this).val('Please wait...');

            $.ajax({
                type: 'POST',
                url: 'http://localhost:8087/ProductsForSale/Create',
                //data: JSON.stringify(list),
                data: JSON.stringify({ purchase: data, products_For_SaleAsList: list }),
                contentType: 'application/json',
                //contentType: false, // Not to set any content header  
                processData: false, // Not to process data 
                success: function (data) {
                    if (data.status) {
                        alert('Successfully saved');
                      
                        //here we will clear the form
                        list = [];
                        alert(($('.purchno').val()).toString());
                        if ($('#rptgnrt').is(":checked") == true) {
                            window.open('http://localhost:8087/api/Purchase/GetPurchaseListReport?prdctno=' + ($('.purchno').val()).toString(), 'name', 'width=600,height=400');
                        }
                        window.open('http://localhost:8087/api/Purchase/GetPurchaseListReport?prdctno=' + ($('.purchno').val()).toString(), 'name', 'width=600,height=400');
                        document.location.reload(true);
                    }
                    else {
                        alert(data.error);
                    }
                    $('#submit').val('Save');
                },
                error: function (error) {
                    console.log(error);
                    $('#submit').val('Save');
                }
            });

        }
    }
});
if ($('#myCheck').is(":checked")) {
    $('.purchno').hide();
    $('#rptshow').hide();
    $('#dlr').hide();

}
else {

    $('.purchno').show();
    $('#rptshow').show();
    $('#dlr').show();

}
function addbox() {

    if ($('#myCheck').is(":checked")) {
        $('.purchno').hide();
        $('#rptshow').hide();
        $('#dlr').hide();

    } else {

        $('.purchno').show();
        $('#rptshow').show();
        $('#dlr').show();

    }
}
