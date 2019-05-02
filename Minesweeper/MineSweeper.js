var tbody = $("#table").find("tbody");

let width;
let height;

var tiles;
var mineNums = [];
var timer=null;

function DrawMap(){ 
    //baseReset
    tbody.html("");
    mineNums = [];
    width = parseInt($('#width').val());
    height = parseInt($('#height').val());
    var mineCount = parseInt($('#mine').val());
    var tileCount = width*height;
    console.log(timer);
    if(timer!=null){ 
        $('.timer').html(0);
        clearInterval(timer);
    }
    timer = setInterval(SetTimer,1000);  console.log(timer);  
    
    tiles = FullIt(tileCount, tile={
        id:0,
        onMine : false,
        nearByMineCount : 0,
        td : null        
    });
    console.log(tiles);
    var i=0;
    var tileNums = FullIt(tileCount); 
    while(i<mineCount){        
        var randNum = Math.floor(Math.random()*(tileNums.length));
        var tilenum = tileNums.splice(randNum, 1);
        mineNums.push(parseInt(tilenum)); //console.log(tileNums);
        i++;        
    }//Setting mine's tileNumber.
    //console.log(mineNums);

    for(i=0; i<height; i++){
        var tr = document.createElement('tr');        
        for(j=0; j<width; j++){
            var tileNum = i*width+j;
            tiles[tileNum].td = document.createElement('td');            
            tiles[tileNum].td.id = tileNum;   
            //tiles[tileNum].safeSearch = function(){ CheckSafeTile(tileNum);}

                if(CheckMine(tileNum)==true){//if tileNum in mineNums.  
                    tiles[tileNum].td.textContent = "★"; //for confirm    
                    tiles[tileNum].td.classList.add("minetd");                  
                    tiles[tileNum].onMine=true;                    
                }//On Mine
            tr.append(tiles[tileNum].td);
        }
        tbody.append(tr);
    }//Draw tile & mine 

    for(index in mineNums){
        var mineNum = mineNums[index];
        var neighborNums = Neighbor(mineNum);
        for(num in neighborNums){  
            var neighberNum = neighborNums[num];          
            SetNearByMineCount(neighberNum);
        }        
        //console.log(neighborNums);
    }//CheckMineCounts per tile that over count one.

    for(indexe in tiles){
        var num = parseInt(indexe);
        var btn = '<button';//btn
        btn += ' id=btn'+num;//id
        btn += ' class=tileBtn';//class
        btn += ' data-num='+num;
        btn += ' data-confirmed='+false;
        btn += '></button>';//end

        $('#'+num).append(btn); //tiles[num].td.append(btn);               
        //console.log(num);            
    }//ButtonDrawing.

    $("button").click(function(){        
        var num = parseInt($(this).attr('data-num'));   
        //console.log(num);     
        //console.log(tiles[num]);          
        if(isNaN(num)==false){ //Not a number filt out
            if(CheckMine(num)==true){
                $(".minetd button").remove(); //by jquery               
                // for(i in mineNums){                    
                //     $('#btn'+mineNums[i]).remove();
                // }
                //$(this).remove();
                
                setTimeout(() => {
                    alert("빵!");       
                    DrawMap();             
                }, 100);
                
            }
            else{
                CheckSafeTile(num);     
                $(this).remove();       
            }// CheckMine 결과에 따라 remove 순서가 다름.     
        }
        CheckRemoveTileCnt();
    });    

    $("button").contextmenu(function(){        
        var btn = $(this);
        
        if(btn.text()==""){
            btn.text('▶');
        }else{
            btn.text('');
        }
        
        return false;
    });   
}

function CheckMine(num){
    if(mineNums.indexOf(num)>-1){
        return true;
    }
    return false;
}

function CheckSafeTile(num){
    var td = $('#'+num).attr('class'); //console.log(td);   
    var nNums = Neighbor(num);
    
    if(td == undefined || td == null){ //safe:no mine a nearby.
       
        var confirmed = $('#btn'+num).attr('data-confirmed');
        //console.log(confirmed);
        if(confirmed == 'false' ){    
            var myConfirmed = $('#btn'+num).attr('data-confirmed','true');    
            for(i in nNums){
                CheckSafeTile(nNums[i]);
                $('#btn'+num).attr('data-confirmed','true');
                $('#btn'+num).remove();                  
            }
        }
        //console.log(btn);
    }else{        
        $('#btn'+num).remove();        
        //console.log("warnningTile");
    }
}

function CheckRemoveTileCnt(){    
    var currentCount = tiles.length - $('.tileBtn').length;
    var goalCount = tiles.length - mineNums.length;
    if(currentCount == goalCount){
        var time = $('.timer').text();
        alert(time);
    }
    var time = $('.timer').text();
    console.log(time);
}

function SetTimer(){    
    var time = parseInt($('.timer').html()); //console.log('time : '+time);    
    time++;    
    $('.timer').html(time);
}

function FullIt(count,object){
    returnVal = Array(count)
        .fill()
        .map((item,index)=>{            
            if(object!=null && object.id!=null){                
                var obj = $.extend({}, object);
                obj.id= index;
                return obj;
            }else{
                return index;
            }
        })
    return returnVal;
}

function Neighbor(number){  
    //console.log(number);
    var tempNeighborNums=[
        number - width -1,//topLeft
        number - width,//top
        number - width +1, // topRight
        number + 1,//right
        number + width +1,//bottomRight
        number + width,//bottom
        number + width -1,//bottomLeft
        number - 1//left
    ]; 

    if((number+1)%width == 0){//rightSide tile
        tempNeighborNums.fill("null",2,5);
    }
    if(number%width == 0){//leftSide tile
        tempNeighborNums[0]="null";
        tempNeighborNums.fill("null",6,8);
    }
    if(number-width < 0){//topSide tile
        tempNeighborNums.fill("null",0,3);
    }
    if(number+width>=tiles.length){//bottomSide tile        
        tempNeighborNums.fill("null",4,7);
    }
    //console.log(number +" / "+tempNeighborNums);
    return tempNeighborNums
}

function SetNearByMineCount(neighberNum){
    if(neighberNum!="null" && tiles[neighberNum].onMine == false){
        tiles[neighberNum].nearByMineCount ++; 
        var mineCount = tiles[neighberNum].nearByMineCount;
        var textColor="black";
        if(mineCount>0){
            switch(mineCount){
                case 1: textColor="black"; break;
                case 2: textColor="green"; break;
                case 3: textColor="blue"; break;
                case 4: textColor="red"; break;
                case 5: textColor="pupple"; break;
                case 6: textColor="yellow"; break;
                case 7: textColor="pink"; break;
                case 8: textColor="white"; break;
            }             
            tiles[neighberNum].td.textContent = mineCount.toString();
            tiles[neighberNum].td.style.color = textColor;
            tiles[neighberNum].td.classList.add("nearbymine");
        }        
        //console.log(tiles[neighberNum].nearByMineCount); 
    }
}
