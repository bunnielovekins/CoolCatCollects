﻿$(function () {
	var form = $(document.forms[0]);
	var copy = form.find("#copy");
	var output = form.find('#output');


	form.submit(function (e) {
		e.preventDefault();

		var url = form.attr('action');
		var data = form.serialize();

		$.post(url, data, function (response) {
			var data = new XMLSerializer().serializeToString(response.documentElement);

			output.show();
			output.val(data);
			copy.show();
			$('#bl').show();
		});

		return false;
	});

	copy.click(function () {
		var op = output[0];

		op.select();
		op.setSelectionRange(0, 99999);
		document.execCommand("copy");
	});

	$('#setQtyApply').click(function () {
		var qty = parseInt($('#setQty').val());

		$('input.qty').each(function () {
			var input = $(this);

			var orig = parseInt(input.attr('data-val'));

			input.val(qty * orig);
		});
	});
});