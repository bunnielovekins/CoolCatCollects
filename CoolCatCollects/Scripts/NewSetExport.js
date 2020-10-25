$(function () {
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

	$('.remark').change(function () {
		$('#exportRemarks').attr('disabled', 'disabled');
	});

	$('#exportRemarksPrepare').click(function () {
		var str = "";

		$('.remark').each(function () {
			var val = $(this).val();
			if (val.length > 0) {
				str += $(this).val() + ",";
			}
		});

		str = str.substr(0, str.length - 1);

		var btn = $('#exportRemarks');

		btn.removeAttr('disabled');
		btn.removeClass('disabled');
		btn.attr('href', btn.attr('data-href') + '&remarks=' + str)

		return false;
	});

	$('#updateDb').click(function () {
		var btn = $(this);

		if (btn.attr('disabled') === 'disabled') {
			return;
		}

		btn.text("Loading...");
		btn.attr('disabled', 'disabled');

		$.post('@Url.Action("UpdateDatabase", "BricklinkCatalog")', $('form').serialize(), function (result) {
			btn.text('Uploaded!');
			btn.removeAttr('disabled')
		});
	});

	$('#showResume').click(() => {
		$('#doResume').show();
		$('#showResume').hide();
		$('#resumeTextBox').show().focus();
	});

	$('#doResume').click(() => {
		var xmlString = $('#resumeTextBox').val();

		if (!xmlString.length) {
			return;
		}

		var xmlObj = parseXml(xmlString, ['parseXml']);
		var items = xmlObj.INVENTORY.ITEM;

		$('tr.part').each((index, elem) => {
			var tr = $(elem);

			var node = getNode(items,
				tr.find('[data-field=category]').val(),
				tr.find('[data-field=colour]').val(),
				tr.find('[data-field=itemId]').val())

			if (node) {
				tr.find('[data-field="check"]').attr('checked', 'checked');
				tr.find('[data-field="qty"]').val(node.qty);
				tr.find('[data-field="price"]').val(node.price);
				tr.find('[data-field="remark"]').val(node.remark);
			}
			else {
				tr.find('[data-field="check"]').removeAttr('checked');
			}
		});

		$('#doResume').hide();
		$('#showResume').show();
		$('#resumeTextBox').hide();
	});
});

function getNode(items, category, colour, itemId) {
	var filtered = items.filter((item) => {
		return item.ITEMID['#text'] == itemId &&
			item.CATEGORY['#text'] == category &&
			item.COLOR['#text'] == colour;
	});

	if (filtered.length) {
		return {
			category: filtered[0].CATEGORY['#text'],
			colour: filtered[0].COLOR['#text'],
			condition: filtered[0].CONDITION['#text'],
			itemId: filtered[0].ITEMID['#text'],
			itemType: filtered[0].ITEMTYPE['#text'],
			price: filtered[0].PRICE['#text'],
			qty: filtered[0].QTY['#text'],
			remark: filtered[0].REMARKS ? filtered[0].REMARKS['#text'] : ''
		};
	}
	return null;
}

function parseXml(xml, arrayTags) {
	var dom = null;
	if (window.DOMParser) {
		dom = (new DOMParser()).parseFromString(xml, "text/xml");
	}
	else if (window.ActiveXObject) {
		dom = new ActiveXObject('Microsoft.XMLDOM');
		dom.async = false;
		if (!dom.loadXML(xml)) {
			throw dom.parseError.reason + " " + dom.parseError.srcText;
		}
	}
	else {
		throw "cannot parse xml string!";
	}

	function isArray(o) {
		return Object.prototype.toString.apply(o) === '[object Array]';
	}

	function parseNode(xmlNode, result) {
		if (xmlNode.nodeName == "#text") {
			var v = xmlNode.nodeValue;
			if (v.trim()) {
				result['#text'] = v;
			}
			return;
		}

		var jsonNode = {};
		var existing = result[xmlNode.nodeName];
		if (existing) {
			if (!isArray(existing)) {
				result[xmlNode.nodeName] = [existing, jsonNode];
			}
			else {
				result[xmlNode.nodeName].push(jsonNode);
			}
		}
		else {
			if (arrayTags && arrayTags.indexOf(xmlNode.nodeName) != -1) {
				result[xmlNode.nodeName] = [jsonNode];
			}
			else {
				result[xmlNode.nodeName] = jsonNode;
			}
		}

		if (xmlNode.attributes) {
			var length = xmlNode.attributes.length;
			for (var i = 0; i < length; i++) {
				var attribute = xmlNode.attributes[i];
				jsonNode[attribute.nodeName] = attribute.nodeValue;
			}
		}

		var length = xmlNode.childNodes.length;
		for (var i = 0; i < length; i++) {
			parseNode(xmlNode.childNodes[i], jsonNode);
		}
	}

	var result = {};
	for (let i = 0; i < dom.childNodes.length; i++) {
		parseNode(dom.childNodes[i], result);
	}

	return result;
}