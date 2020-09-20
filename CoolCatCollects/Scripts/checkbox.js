$(function () {
	$('tr.checkable').click(function () {
		var checkbox = $(this).find('label.check input');
		check(checkbox);
	});

	$('td.checkable').click(function () {
		var tr = $(this).parent();
		var checkbox = tr.find('label.check input');
		check(checkbox);
	});
});


function check(checkbox) {
	if (checkbox.prop('checked')) {
		checkbox.prop('checked', false);
	} else {
		checkbox.prop('checked', true);
	}
}
