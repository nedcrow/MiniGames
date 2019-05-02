var tbody = $('#table').find('tbody');

let width; //width tile count
let height; //height tile count

var tiles; //total tile objects
var mineNums = []; //mine tile numbers
var timer=null; //timer table tag in html

var currentTargetTD; //cursor overed td in html
var clickCount; //

function DrawMap(time, clickNumber){ 
    //baseReset
    tbody.html('');
    mineNums = [];
    width = parseInt($('#width').val());
    height = parseInt($('#height').val());
    var mineCount = parseInt($('#mine').val());
    var tileCount = width*height;
    
    if(timer!=null){ 
        if(time!=undefined){//case that restart as soon as touch.
            $('#timer').html(time);
        }else{//normal case
            $('#timer').html(0);
        }
        clearInterval(timer);
    }
    timer = setInterval(()=>{
        SetTimer();
    },1000); //SetTimer
    
    tiles = FullIt(tileCount, tile={
        id:0,
        onMine : false, //For SetNearByMineCount
        nearByMineCount : 0, //For SetNearByMineCount
        td : null  // For //Draw tile & mine
    }); //console.log(tiles);
    
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
            tiles[tileNum].td.id = 'td'+tileNum;               
            if (CheckMine(tileNum) == true) {//if tileNum in mineNums.  
                tiles[tileNum].td.textContent = '★'; //for confirm    
                tiles[tileNum].td.classList.add('minetd');               
                tiles[tileNum].onMine = true;
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
        btn += ' data-num='+num;//data-num
        btn += ' data-confirmed='+false;//data-confirmed
        btn += '></button>';//end

        $('#td'+num).append(btn);              
        //console.log(num);            
    }//TileButtonDrawing.

    $('td').mouseover((ev)=>{
        var targetID =  $(ev.target).attr('id');
        if(targetID.slice(0,2)=='td'){
            currentTargetTD = $('#'+targetID);    
        }
        //console.log(currentTargetTD);
    }); //currentTargetTD select

    $('td').mouseout((ev)=>{
        var targetID =  $(ev.target).attr('id');        
    }); //currentTargetTD unSelect

    $('button').click(function(){
        var tileNumber = parseInt($(this).attr('data-num'));  
        TileClick(tileNumber);
    });    

    $('button').contextmenu(function(){ //If RightClick       
        var btn = $(this);
        
        if(btn.text()==''){
            btn.text('▶');
        }else{
            btn.text('');
        }        
        return false;
    });   

    $('td').mousedown(function(e) { 
        if(e.which == 2){//If WheelClick  (1:left, 3:right)
            //console.log(currentTargetTD);            
            let id = parseInt(currentTargetTD.attr('data-num'));
            let neighborNums = Neighbor(id);
            neighborNums.push(id);
            let nearbymine = false;
            let btns=[];
            
            for(i in neighborNums){
                let className = $('#td'+neighborNums[i]).attr('class');
                if(className=='minetd'){//this td is mine.
                    if($('#btn'+neighborNums[i]).text()!='▶'){//this btn was not flagged.
                        nearbymine=true;
                    }
                }else{
                    if($('#btn'+neighborNums[i]).attr('id')!=undefined){//this btn is here.
                        console.log($('#btn'+neighborNums[i]).attr('id'));
                        btns.push( $('#btn'+neighborNums[i]));
                    }
                }
            }
            console.log(nearbymine);
            if(nearbymine == false){
                for(i in btns){
                    btns[i].remove();
                }
            }
        }
    });

    //if restart (첫 클릭으로 지뢰면 Gameover아님.)
    console.log(clickNumber);
    if(clickNumber>=0){
        TileClick(clickNumber);
        console.log('restart');
    }
}//DrawMap & GameStart

function SetTimer(){    
    var time = parseInt($('#timer').html()); //console.log('time : '+time);  
    time++;     
    $('#timer').html(time);
}//timer 값을 1만큼 증가시킴.

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
}//array를 count만큼 object로 체움. 

function CheckMine(num){
    if(mineNums.indexOf(num)>-1){
        return true;
    }
    return false;
}//해당 num의 tile에 mine이 있으면 true, 없으면 false 리턴.

function TileClick(tileNumber){  //If LeftClick      
    var num = parseInt(tileNumber);   
    if(isNaN(num)==false){ //Not a number filt out.
        if(CheckMine(num)==true){//Mine touched.
            if(clickCount>0){
                $('.minetd button').remove(); //by jquery.               
                // for(i in mineNums){                    
                //     $('#btn'+mineNums[i]).remove();
                // }                
                setTimeout(() => {
                    alert('빵!');  
                    clickCount = 0; 
                    DrawMap();             
                }, 100);     
            }//GameOver
            else{
                console.log(num);
                DrawMap(
                    parseInt($('#timer').html()),
                    parseInt(num)
                )
            }//GameRestart
        }
        else{
            CheckSafeTile(num);     
            $(this).remove();       
        }// CheckMine 결과에 따라 remove 순서가 다름.     
    }
    CheckEnding();
    clickCount++;
}//tile을 click함.

function CheckSafeTile(num){
    var td = $('#td'+num).attr('class'); //console.log(td);   
    var nNums = Neighbor(num);
    
    if(td == undefined || td == null){ //safe:no mine a nearby.
       
        var confirmed = $('#btn'+num).attr('data-confirmed');        
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
        //console.log('warnningTile');
    }
}//mine과 떨어진 tile들을 전부 확인하고 btn 삭제.

function CheckEnding(){    
    var currentCount = tiles.length - $('.tileBtn').length;
    var goalCount = tiles.length - mineNums.length;
    if(currentCount == goalCount){
        var time = $('.timer').text();
        alert(time);
        clickCount=0;
    }
    var time = $('.timer').text();
}//removeTileCount를 참고하여 clear 시 ending popup.


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
        tempNeighborNums.fill('null',2,5);
    }
    if(number%width == 0){//leftSide tile
        tempNeighborNums[0]='null';
        tempNeighborNums.fill('null',6,8);
    }
    if(number-width < 0){//topSide tile
        tempNeighborNums.fill('null',0,3);
    }
    if(number+width>=tiles.length){//bottomSide tile        
        tempNeighborNums.fill('null',4,7);
    }
    //console.log(number +' / '+tempNeighborNums);
    return tempNeighborNums
}//return neighbor's number per target tile.

function SetNearByMineCount(neighberNum){
    if(neighberNum!='null' && tiles[neighberNum].onMine == false){
        tiles[neighberNum].nearByMineCount ++; 
        var mineCount = tiles[neighberNum].nearByMineCount;
        var textColor='black';
        if(mineCount>0){
            switch(mineCount){
                case 1: textColor='black'; break;
                case 2: textColor='green'; break;
                case 3: textColor='blue'; break;
                case 4: textColor='red'; break;
                case 5: textColor='pupple'; break;
                case 6: textColor='yellow'; break;
                case 7: textColor='pink'; break;
                case 8: textColor='white'; break;
            }             
            tiles[neighberNum].td.textContent = mineCount.toString();
            tiles[neighberNum].td.style.color = textColor;
            tiles[neighberNum].td.classList.add('nearbymine');
        }        
        //console.log(tiles[neighberNum].nearByMineCount); 
    }
}//target tile의 textContents에 'nearby mine count' 입력.
