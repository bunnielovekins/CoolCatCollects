$(function () {
	var form = $(document.forms[0]);
	var copy = form.find("#copy");
	var output = form.find('#output');

	$('.js-export-copy').click(function () {
		var url = $(this).attr('data-href');
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

		$('tr.part').each(function () {
			var tr = $(this);
			var input = tr.find('input.qty');
			var btn = tr.find('.js-break');
			var orig = +input.attr('data-val');
			var newVal = qty * orig;

			input.val(newVal);
			btn.attr('data-qty', newVal);
		});
	});

	$('#updateDb').click(function () {
		var btn = $(this);

		if (btn.attr('disabled') === 'disabled') {
			return;
		}

		btn.text("Loading...");
		btn.attr('disabled', 'disabled');

		$.post(updateDbUrl, $('form').serialize(), function (result) {
			btn.text('Uploaded!');
			btn.removeAttr('disabled')
		});
	});

	$('.js-break').click(function () {
		var btn = $(this);

		var number = btn.attr('data-number');
		var colour = btn.attr('data-colour');
		var type = btn.attr('data-type');
		var qty = +btn.attr('data-qty');

		var table = btn.parents('table');

		btn.attr('disabled', 'disabled');
		btn.text('Loading...');

		$.post(subsetUrl, { number, colour, type }, (response) => {
			var parts = response.Parts;

			parts.forEach(part => {
				var tr = $('tr.part[data-number=' + part.Number + '][data-colour=' + part.ColourId + '][data-type=' + part.Type + ']');
				if (tr.length) {
					ConsolidateNewPart(tr, part, qty);
				}
				else {
					AddNewRow(table, part, qty);
				}
			});

			btn.parents('tr').remove();

			RecalculateIndexes();
		});
	});

	$('.js-reorder').click(() => {
		var arr = $('tr.part').map((i, x) => {
			return {
				i: $(x).attr('data-i'),
				status: $(x).attr('data-status'),
				colour: $(x).attr('data-colourname'),
				number: $(x).attr('data-number'),
				elem: $(x)
			};
		}).toArray();

		var stringCompare = (first, second) => {
			return first.toLowerCase().localeCompare(second.toLowerCase());
		}

		arr = arr.sort((first, second) => {
			if (first.status !== second.status) {
				return stringCompare(first.status, second.status);
			}

			if (first.colour !== second.colour) {
				return stringCompare(first.colour, second.colour);
			}

			return stringCompare(first.number, second.number);
		});

		arr.map(x => x.elem).reverse().forEach(function (elem) {
			$(elem).prependTo($('table.partstable'));
		});

		RecalculateIndexes();
	});

	$('.js-resume-upload').click(function () {
		readXmlFromFile().then(function (xmlString) {
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

			$('#resume-dialog').modal('hide');
		}, function (err) {
			// failed
			console.log("Failed");
			console.log(err);
		})
	});

	$('#resumeInput').change(function () {
		$('.js-resume-upload').removeClass('disabled').removeAttr('disabled');
	});

	$('.js-weight').each(function () { getPartWeight($(this)); });
});

function RecalculateIndexes() {
	$('tr.part').each((index, elem) => {
		var tr = $(elem);
		var indexBefore = tr.attr('data-i');

		tr.attr('data-i', index);
		tr.find('input').each((inpIndex, inpElem) => {
			var input = $(inpElem);

			input.attr('name', input.attr('name').replace('[' + indexBefore + ']', '[' + index + ']'));
		});
	});
}

function ConsolidateNewPart(tr, part, qty) {
	var qtyInput = tr.find('.qty');
	qtyInput.attr('data-val', +qtyInput.attr('data-val') + part.Quantity);
	qtyInput.val(+qtyInput.val() + (part.Quantity * qty));
}

function getPartWeight (td) {
	$.get(td.attr('data-href')).then(function (res) {
		td.text(res);
	}, function () {
		td.text('---');
	});
}

function AddNewRow(table, part, qty) {
	var lastRow = table.find('tr:last');

	var i = (+lastRow.attr('data-i'));
	var newIndex = i + 1;

	var newRow = lastRow.clone(true, true);

	newRow.find('input')
		.each(function () {
			var elem = $(this);
			var name = elem.attr('name').replace('[' + i + ']', '[' + newIndex + ']');
			elem.attr('name', name);
		});

	table.append(newRow);

	newRow = table.find('tr:last');

	newRow
		.attr('data-i', newIndex)
		.attr('data-colour', part.ColourId)
		.attr('data-number', part.Number)
		.attr('data-type', part.Type)
		.attr('data-status', part.Status ?? '')
		.attr('data-colourname', part.ColourName);

	newRow.find('[data-field=itemType]').val(part.Type[0]);
	newRow.find('[data-field=category]').val(part.Category);
	newRow.find('[data-field=colour]').val(part.ColourId);
	newRow.find('[data-field=itemId]').val(part.Number);
	newRow.find('[data-field=check]').attr('checked', 'checked');
	newRow.find('img').attr('src', part.Image);
	newRow.find('.name').empty().text(part.Name);
	newRow.find('.no').empty().text(part.Number);
	newRow.find('.type').empty().text(part.Type);
	if (part.Colour) {
		newRow.find('.colour').empty().attr('title', part.ColourName).append(
			$('<span class="colbox" style="background-color: #' + part.Colour.ColourCode + '; color: #' + part.Colour.ColourCode + ';">&nbsp;</span>')
		);
		newRow.find('.colourname').empty().text(part.ColourName);
	}
	else {
		newRow.find('.colour').empty();
		newRow.find('.colourname').empty();
	}

	newRow.find('.qty')
		.attr('data-val', part.Quantity)
		.val(qty * part.Quantity);
	newRow.find('.remark')
		.attr('data-val', part.Remark)
		.val(part.Remark);
	newRow.find('.price')
		.attr('data-val', part.MyPrice)
		.val(part.MyPrice);
	newRow.find('.avgprice')
		.empty().text(part.AveragePrice);

	newRow.find('.js-weight')
		.attr('data-href', '/BricklinkCatalog/GetWeight?num=' + part.Number + '&type=' + part.Type);

	getPartWeight(newRow.find('.js-weight'));
}

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

function readXmlFromFile() {
	return new Promise(function (resolve, reject) {
		var file = $('#resumeInput').prop('files')[0];

		if (!file) {
			reject();
		}

		var reader = new FileReader();
		reader.onload = function (e) {
			var xml = e.target.result;

			resolve(xml);
		};

		reader.readAsText(file);
	});
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