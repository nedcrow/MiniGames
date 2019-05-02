console.log($("table"));

var str = "";
str += "<tr><td>";
for(var i; i < 100; i++){
    str += "<button id='btn"+i+"'>";
    str += "</button></td>"
    
    if(i % 10 == 0){
        str += "</tr>"
    }
}

$("table").append(str);