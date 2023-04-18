$(document).ready(function () {
        $(function () {
            $(".add-to-cart").click(function () {
                var productid = $(this).find('.Id').val();
                var soLuong = $(document).find('#txtsoLuong').val();
                $.ajax({
                    url: '/api/cart/add',
                    type: "POST",
                    dataType: "JSON",
                    data: {
                        productID: productid,
                        amount: soLuong
                    },
                    success: function (response) {
                        if (response.result == 'Redirect') {
                            window.location = response.url;
                        }
                        else {                               
                            loadHeaderCart(); //Add Product success
                            location.reload();
                        }
                        console.log(response); // log to the console to see whether it worked
                    },
                    error: function (error) {
                        alert("There was an error posting the data to the server: " + error.responseText);
                    }
                });
                console.log(productid, soLuong)
            });          
        });
    function loadHeaderCart() {
        $('#miniCart').load("/AjaxContent/HeaderCart");
    $('#numberCart').load("/AjaxContent/NumberCart");
        }
    });