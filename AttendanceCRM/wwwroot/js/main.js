// CSS Which needs to add
var css = [
"css/introjs.css",
"css/bootstrap.min.css",
"css/datepicker.css",
"css/bootstrap-select.min.css",
"css/style.css",
"css/dev-style.css",
"https://fonts.googleapis.com/css?family=Poppins:300,300i,400,400i,500,600,700,900&display=swap"
];

// JS Which needs to add
var js = [
"js/intro.js",
"js/bootstrap.bundle.min.js",
"js/bootstrap-datepicker.js",
"js/bootstrap-select.min.js",
"js/jquery.nicescroll.min.js",
"js/custom.js"
];


// Add All css and js to head in HTML page
var headID = document.getElementsByTagName('head')[0];
for(i=0; i<css.length; i++){
	var link = document.createElement('link');
	link.href = css[i];
	link.type="text/css";
	link.rel = "stylesheet";	
	headID.appendChild(link);
};

var script = document.createElement('script');
script.src = "js/jquery.js";
script.type="text/javascript";
script.onload = function(){
	for(j=0; j<js.length; j++){
		var script = document.createElement('script');
		script.src = js[j];
		script.type="text/javascript";
		headID.appendChild(script);
	};	
}
headID.appendChild(script);