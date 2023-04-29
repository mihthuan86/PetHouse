$('.plus').on('click', function () {
	if ($(this).prev().val()) {
		$(this).prev().val(+$(this).prev().val() + 1);
	}
});
$('.minus').on('click', function () {
	if ($(this).next().val() > 1) {
		if ($(this).next().val() > 1) $(this).next().val(+$(this).next().val() - 1);
	}
});