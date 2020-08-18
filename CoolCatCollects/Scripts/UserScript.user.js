// ==UserScript==
// @name     Bricklink Packer
// @version  1
// @require https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js
// ==/UserScript==

var $ = jQuery;
$('<div id="GM_Packed" style="position: fixed; bottom:10px; left: 10px;"><button>Packed</button></div>').appendTo($('body'));
$('<div id="GM_Shipped" style="position: fixed; bottom:10px; right: 10px;"><button>Shipped</button></div>').appendTo($('body'));

$('#GM_Packed').click(function () {
	$('select.bl-form-select[name^="nS"').each(function () {
		var elem = $(this);

		if (elem.val() !== '3') {
			return;
		}

		elem.val('5');
	});
	return false;
});

$('#GM_Shipped').click(function () {
	$('select.bl-form-select[name^="nS"').each(function () {
		var elem = $(this);

		if (elem.val() !== '5') {
			return;
		}

		elem.val('7');
	});
	return false;
});