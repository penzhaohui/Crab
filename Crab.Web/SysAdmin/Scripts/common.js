function $(id) {
	return document.getElementById(id);
}

var tooltip;
document.write("<div id='js_tooltip' style='position:absolute; z-index:1000; visibility:hidden'></div>");
function popLayer(e) {
	var el = e.srcElement ? e.srcElement : e.target;
	if (!el) {
		return;
	}
	if (el.alt != null && el.alt != '') {
		el.tooltip = el.alt;
		el.alt = '';
	}
	if (el.title != null && el.title != '') {
		el.tooltip = el.title;
		el.title = '';
	}
	if (el.tooltip != tooltip) {
		tooltip = el.tooltip;
		var container = $('js_tooltip');
		if (tooltip == null || tooltip == '') {
			container.style.visibility = 'hidden';
		} else {
			container.style.visibility = 'visible';
			container.innerHTML = tooltip.replace(/\n/g, '<br>');
			var mouse = { x : e.clientX, y : e.clientY };
			var size = { w : container.clientWidth, h : container.clientHeight };
			var adjustLeft  = mouse.x + 12 + size.w > document.body.clientWidth ?  - size.w - 12 : 12;
			var adjustRight = mouse.y + 12 + size.h > document.body.clientHeight ? - size.h - 12 : 12;
			container.style.left = (mouse.x + document.body.scrollLeft + adjustLeft) + 'px';
			container.style.top  = (mouse.y + document.body.scrollTop + adjustRight) + 'px';
		}
	}
}
document.onmouseover = function(e) {
	popLayer(e ? e : event);
}

function popUpMenu()
{
	var divPop = document.getElementById('PopUpMenu');
	var lbtnMode = document.getElementById('lbtnSelectMod');
    
	divPop.style.visibility = 'visible';
	divPop.style.left = event.srcElement.offsetLeft + 180; 
	divPop.style.top = event.srcElement.offsetTop+event.srcElement.offsetHeight + 150;
}
function mouseout()
{
	var divPop = document.getElementById('PopUpMenu');
	var lbtnMode = document.getElementById('lbtnSelectMod');
	divPop.style.visibility = 'hidden';
}
function mouseover()
{
	var divPop = document.getElementById('PopUpMenu');
	var lbtnMode = document.getElementById('lbtnSelectMod');
    
	divPop.style.visibility = 'visible';
}
function mouseoutdiv()
{
	var divPop = document.getElementById('PopUpMenu');
	var lbtnMode = document.getElementById('lbtnSelectMod');
	divPop.style.visibility = 'hidden';
}


function selectAllCheckboxes(spanChk, strChildSubId)
{
    var oItem = spanChk.children;
　  var theBox = (spanChk.type == "checkbox") ? spanChk : spanChk.children.item[0];
　  var xState = theBox.checked;
　  var elm = theBox.form.elements;
　  var i = 0;
　  for(i = 0; i < elm.length; i++) {
　      if(elm[i].type == "checkbox" && elm[i].id != theBox.id && elm[i].id.indexOf(strChildSubId)>=0)
　      {
　　        if(elm[i].checked != xState)
　　        {
　　            elm[i].click();
　　            elm[i].checked = xState;
　　        }
　      }
　  }
}