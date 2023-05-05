$(function () {
    function loadHeaderCart() {
        $('#miniCart').load("/AjaxContent/HeaderCart");
        $('#numberCart').load("/AjaxContent/NumberCart");
    }
    $(".product-remove").click(function () {
        var productid = $(this).attr("data-mahh");
        $.ajax({
            url: "api/cart/remove",
            type: "POST",
            dataType: "JSON",
            data: { productID: productid },
            success: function (result) {
                if (result.success) {
                    loadHeaderCart();//Reload lai gio hang
                    location.reload();
                }
            },
            error: function (rs) {
                alert("Remove Cart Error !")
            }
        });
    });
    $(".update-btn").click(function () {
        var productid = $(this).attr("data-mahh");
        if ($(this).hasClass("plus")) {
            var _url = "api/cart/plus"
        }
        else {
            var _url = "api/cart/minus"
        }
        var soluong = parseInt($(this).val());
        $.ajax({
            url: _url,
            type: "POST",
            dataType: "JSON",
            data: {
                productID: productid,
                amount: soluong
            },
            success: function (result) {
                if (result.success) {
                    loadHeaderCart();//Reload lai gio hang
                    window.location = 'cart.html';
                }
            },
            error: function (rs) {
                alert("Cập nhật Cart Error !")
            }
        });
    });
});
function updateCart(element) {
    var productId = element.getAttribute("data-mahh");
    var soluong = element.value;
    if (soluong < 1) {
        alert('Vui lòng chọn lại số lượng');
    }
    else {
        $.ajax({
            url: "api/cart/update",
            type: "POST",
            dataType: "JSON",
            data: {
                productID: productId,
                amount: soluong
            },
            success: function (result) {
                if (result.success) {
                    $('#numberCart').load("/AjaxContent/NumberCart");
                    window.location = 'cart.html';
                }
            },
            error: function (rs) {
                alert("Cập nhật Cart Error !")
            }
        });
    }
}
//$(function () {
//    function loadHeaderCart() {
//        $('#numberCart').load("/AjaxContent/NumberCart");
//    }
//    // function to update cart with given product id and quantity
//    function updateCart(productId, quantity) {
//        $.ajax({
//            url: "api/cart/update",
//            type: "POST",
//            dataType: "JSON",
//            data: {
//                productID: productId,
//                amount: quantity
//            },
//            success: function (result) {
//                if (result.success) {
//                    $('#numberCart').load("/AjaxContent/NumberCart");
//                    window.location = 'cart.html';
//                }
//            },
//            error: function (rs) {
//                alert("Cập nhật Cart Error !")
//            }
//        });
//    }

//    // function to update cart when plus or minus button is clicked
//    function updateCartOnButtonClick(element, quantity) {
//        var productId = element.getAttribute("data-mahh");
//        updateCart(productId, quantity);
//    }

//    $(".product-remove").click(function () {
//        var productid = $(this).attr("data-mahh");
//        $.ajax({
//            url: "api/cart/remove",
//            type: "POST",
//            dataType: "JSON",
//            data: { productID: productid },
//            success: function (result) {
//                if (result.success) {
//                    loadHeaderCart();//Reload lai gio hang
//                    location.reload();
//                }
//            },
//            error: function (rs) {
//                alert("Remove Cart Error !")
//            }
//        });
//    });

//    // handle click on plus button
//    $(".plus").click(function () {
//        var input = $(this).siblings(".qty");
//        var quantity = parseInt(input.val()) + 1;
//        input.val(quantity);
//        updateCartOnButtonClick(this, quantity);
//    });

//    // handle click on minus button
//    $(".minus").click(function () {
//        var input = $(this).siblings(".qty");
//        var quantity = parseInt(input.val()) - 1;
//        if (quantity < 1) {
//            quantity = 1;
//        }
//        input.val(quantity);
//        updateCartOnButtonClick(this, quantity);
//    });

//    $(".update-btn").click(function () {
//        var productid = $(this).attr("data-mahh");
//        var soluong = parseInt($(this).val());
//        updateCart(productid, soluong);
//    });
//});