function drawLayer02Label(canvasObj,text,textBeginX,lineEndX){
	var colorValue = '#04918B';

	var ctx = canvasObj.getContext("2d");

	ctx.beginPath();
	ctx.arc(35,55,2,0,2*Math.PI);
	ctx.closePath();
	ctx.fillStyle = colorValue;
	ctx.fill();

	ctx.moveTo(35,55);
	ctx.lineTo(60,80);
	ctx.lineTo(lineEndX,80);
	ctx.lineWidth = 1;
	ctx.strokeStyle = colorValue;
	ctx.stroke();

	ctx.font='12px Georgia';
	ctx.fillStyle = colorValue;
	ctx.fillText(text,textBeginX,92);
}

//接入机型占比

var COLOR = {
	MACHINE:{
		TYPE_A:'#D68780',
		TYPE_B:'#F2CCA8',
		TYPE_C:'#D34864',
		TYPE_D:'#A3B698',
		TYPE_E:'#FFFFFF',
		TYPE_F:'#009E9A',
		TYPE_G:'#AC266F'
	}
};

function renderLegend(){
	drawLegend(COLOR.MACHINE.TYPE_A,25,'学习');
	drawLegend(COLOR.MACHINE.TYPE_B,50,'工作');
	drawLegend(COLOR.MACHINE.TYPE_C,75,'浪费');
	drawLegend(COLOR.MACHINE.TYPE_D,100,'休息');
	drawLegend(COLOR.MACHINE.TYPE_E,125,'其他');
}

function drawLegend(pointColor,pointY,text){
	var ctx = $("#layer03_left_01 canvas").get(0).getContext("2d");
	ctx.beginPath();
	ctx.arc(20,pointY,6,0,2*Math.PI);
	ctx.fillStyle = pointColor;
	ctx.fill();
	ctx.font='20px';
	ctx.fillStyle = '#FEFFFE';
	ctx.fillText(text,40,pointY+3);
}

function drawLayer03Right(canvasObj,colorValue,rate){
	var ctx = canvasObj.getContext("2d");
    
	var circle = {
        x : 65,    //圆心的x轴坐标值
        y : 80,    //圆心的y轴坐标值
        r : 60      //圆的半径
    };
	ctx.clearRect(0,0,120,120);
	//画扇形
	//ctx.sector(circle.x,circle.y,circle.r,1.5*Math.PI,(1.5+rate*2)*Math.PI);
	//ctx.fillStyle = colorValue;
	//ctx.fill();

	ctx.beginPath();
	ctx.arc(circle.x,circle.y,circle.r,0,Math.PI*2)
	ctx.lineWidth = 10;
	ctx.strokeStyle = '#052639';
	ctx.stroke();
	ctx.closePath();

	ctx.beginPath();
	ctx.arc(circle.x,circle.y,circle.r,1.5*Math.PI,(1.5+rate*2)*Math.PI)
	ctx.lineWidth = 10;
	ctx.lineCap = 'round';
	ctx.strokeStyle = colorValue;
	ctx.stroke();
	ctx.closePath();
    
	ctx.fillStyle = 'white';
	ctx.font = '20px Calibri';
	ctx.fillText(rate*1000/10+'%',circle.x-15,circle.y+10);

}

//存储
function renderLayer03Right(data){
	let total = (Number(data.work) + Number(data.rest) + Number(data.study) + Number(data.waste) + Number(data.other)).toFixed(2);
	drawLayer03Right($("#layer03_right_chart01 canvas").get(0),COLOR.MACHINE.TYPE_A,Number(data.study/total).toFixed(2));
	drawLayer03Right($("#layer03_right_chart02 canvas").get(0),COLOR.MACHINE.TYPE_B,Number(data.work/total).toFixed(2));
	drawLayer03Right($("#layer03_right_chart03 canvas").get(0),COLOR.MACHINE.TYPE_C,Number(data.waste/total).toFixed(2));
}

function renderChartBar01(data){
	var myChart = echarts.init(document.getElementById("layer03_left_02"));
	myChart.setOption(
	{
		title : {
			text: '',
			subtext: '',
			x:'center'
		},
		tooltip : {
			trigger: 'item',
			formatter: "{b} : {c} ({d}%)"
		},
		legend: {
			show:false,
			x : 'center',
			y : 'bottom',
			data:['学习','工作','浪费','休息','其他']
		},
		toolbox: {
		},
		label:{
			normal:{
				show: true, 
				formatter: "{b} \n{d}%"
			} 
		},
		calculable : true,
		color:[COLOR.MACHINE.TYPE_A,COLOR.MACHINE.TYPE_B,COLOR.MACHINE.TYPE_C,COLOR.MACHINE.TYPE_D,COLOR.MACHINE.TYPE_E],
		series : [
			{
				name:'',
				type:'pie',
				radius : [40, 80],
				center : ['50%', '50%'],
				//roseType : 'area',
				data:[
					{value:data.study, name:'学习'},
					{value:data.work, name:'工作'},
					{value:data.waste, name:'浪费'},
					{value:data.rest, name:'休息'},
					{value:data.other, name:'其他'},
				]
			}
		]
	});
}

/*
function renderChartBar02(){
	var myChart = echarts.init(document.getElementById("layer03_left_03"));
		myChart.setOption(
					{
						title : {
							text: '',
							subtext: '',
							x:'center'
						},
						tooltip : {
							show:true,
							trigger: 'item',
							formatter: "上线率<br>{b} : {c} ({d}%)"
						},
						legend: {
							show:false,
							orient: 'vertical',
							left: 'left',
							data: ['A机型','B机型','C机型','D机型','E机型','F机型','G机型']
						},
						series : [
							{
								name: '',
								type: 'pie',
								radius : '50%',
								center: ['50%', '60%'],
								data:[
									{value:7600, name:'A机型'},
									{value:6600, name:'B机型'},
									{value:15600, name:'C机型'},
									{value:5700, name:'D机型'},
									{value:4600, name:'E机型'},
									{value:4600, name:'F机型'},
									{value:3500, name:'G机型'}
								],
								itemStyle: {
									emphasis: {
										shadowBlur: 10,
										shadowOffsetX: 0,
										shadowColor: 'rgba(0, 0, 0, 0.5)'
									}
								}
							}
						],
						color:[COLOR.MACHINE.TYPE_A,COLOR.MACHINE.TYPE_B,COLOR.MACHINE.TYPE_C,COLOR.MACHINE.TYPE_D,COLOR.MACHINE.TYPE_E,COLOR.MACHINE.TYPE_F,COLOR.MACHINE.TYPE_G]
					}
		);
}*/

function renderLayer04Left(seriesData){
	var myChart = echarts.init(document.getElementById("layer04_left_chart"));
	myChart.setOption(
		{
			title: {
				text: ''
			},
			tooltip : {
				trigger: 'axis'
			},
			legend: {
				data:[]
			},
			grid: {
				left: '3%',
				right: '4%',
				bottom: '5%',
				top:'4%',
				containLabel: true
			},
			xAxis :
			{
				type : 'category',
				boundaryGap : false,
				data : getLatestDays(seriesData.startDate,seriesData.endDate),
				axisLabel:{
					textStyle:{
						color:"white", //刻度颜色
						fontSize:8  //刻度大小
					},
					rotate:45,
					interval:2
				},
				axisTick:{show:false},
				axisLine:{
					show:true,
					lineStyle:{
						color: '#0B3148',
						width: 1,
						type: 'solid'
					}
				}
			},
			yAxis : 
			{
				type : 'value',
				axisTick:{show:false},
				axisLabel:{
					textStyle:{
						color:"white", //刻度颜色
						fontSize:8  //刻度大小
						}
				},
				axisLine:{
					show:true,
					lineStyle:{
						color: '#0B3148',
						width: 1,
						type: 'solid'
					}
				},
				splitLine:{
					show:false
				}
			},
			tooltip:{
				formatter:'{c}',
				backgroundColor:'#FE8501'
			},
			series : [
				{
					name:'',
					type:'line',
					smooth:true,
					areaStyle:{
						normal:{
							color:new echarts.graphic.LinearGradient(0, 0, 0, 1, [{offset: 0, color: '#026B6F'}, {offset: 1, color: '#012138' }], false),
							opacity:0.2
						}
					},
					itemStyle : {  
                            normal : {  
                                  color:'#009991'
                            },
							lineStyle:{
								normal:{
								color:'#009895',
								opacity:1
							}
						}
                    },
					symbol:'none',
					data:getLatestDays(seriesData.startDate,seriesData.endDate).map(function(value){
						　　return seriesData.study[value];
						})
				}
			]
		}
	
	);
}
function renderLayer04Right(seriesData){
	var myChart = echarts.init(document.getElementById("layer04_right_chart"));
	myChart.setOption({
			title: {
				text: ''
			},
			tooltip: {
				trigger: 'axis'
			},
			legend: {
				top:20,
				right:5,
				textStyle:{
					color:'white'
				},
				orient:'vertical',
				data:[
						{name:'工作',icon:'circle'},
						{name:'学习',icon:'circle'},
						{name:'浪费',icon:'circle'}
					]
			},
			grid: {
				left: '3%',
				right: '16%',
				bottom: '3%',
				top:'3%',
				containLabel: true
			},
			xAxis: {
				type: 'category',
				boundaryGap: false,
				axisTick:{show:false},
				axisLabel:{
					textStyle:{
						color:"white", //刻度颜色
						fontSize:8  //刻度大小
						}
				},
				axisLine:{
					show:true,
					lineStyle:{
						color: '#0B3148',
						width: 1,
						type: 'solid'
					}
				},
				data: getLatestDays(seriesData.startDate,seriesData.endDate)
			},
			yAxis: {
				type: 'value',
				axisTick:{show:false},
				axisLabel:{
					textStyle:{
						color:"white", //刻度颜色
						fontSize:8  //刻度大小
						}
				},
				axisLine:{
					show:true,
					lineStyle:{
						color: '#0B3148',
						width: 1,
						type: 'solid'
					}
				},
				splitLine:{
					show:false
				}
			},
			series: [
						{
							name:'工作',
							type:'line',
							itemStyle : {  
									normal : {  
									color:'#F3891B'
								},
								lineStyle:{
									normal:{
									color:'#F3891B',
									opacity:1
										}
								}
							},  
							data:getLatestDays(seriesData.startDate,seriesData.endDate).map(function(value){
								　　return ((isNaN(seriesData.work[value]) || seriesData.work[value]===undefined)?0:seriesData.work[value]).toFixed(2);
								})
						},
						{
							name:'学习',
							type:'line',
							itemStyle : {  
									normal : {  
									color:'#006AD4'
								},
								lineStyle:{
									normal:{
									color:'#F3891B',
									opacity:1
										}
								}
							},
							data:getLatestDays(seriesData.startDate,seriesData.endDate).map(function(value){
								　　return ((isNaN(seriesData.study[value])||seriesData.study[value]===undefined)?0:seriesData.study[value]).toFixed(2);
								})
						},
						{
							name:'浪费',
							type:'line',
							itemStyle : {  
									normal : {  
									color:'#FF0000'
								},
								lineStyle:{
									normal:{
									color:'#FF0000',
									opacity:1
										}
								}
							},
							data:getLatestDays(seriesData.startDate,seriesData.endDate).map(function(value){
								　　return ((isNaN(seriesData.waste[value])||seriesData.waste[value]===undefined)?0:seriesData.waste[value]).toFixed(2);
								})
						}
					]
		}	
	);
}

function get10MinutesScale()
{
	var currDate = new Date();
	var odd = currDate.getMinutes()%10;
	var returnArr = new Array();
	currDate.setMinutes(currDate.getMinutes()-odd);
	for(var i = 0; i <7; i++){
		returnArr.push(currDate.getHours()+":"+(currDate.getMinutes()<10?("0"+currDate.getMinutes()):currDate.getMinutes()));
		currDate.setMinutes(currDate.getMinutes()-10);
	}
	return returnArr;
}
Date.prototype.Format = function (fmt) {
	var o = {
		"M+": this.getMonth() + 1,//月份
		"d+": this.getDate(),//日
		"H+": this.getHours(),//小时
		"m+": this.getMinutes(),//分
		"s+": this.getSeconds(),//秒
		"q+": Math.floor((this.getMonth() + 3) / 3),//季度
		"S+": this.getMilliseconds()//毫毛
	};
	if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
	for (var k in o)
		if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
	return fmt;
};
function dateAddDays(dataStr,dayCount) {
    var strdate=dataStr; //日期字符串
    var isdate = new Date(strdate.replace(/-/g,"/"));  //把日期字符串转换成日期格式
    isdate = new Date((isdate/1000+(86400*dayCount))*1000);  //日期加1天
    var pdate = isdate.Format("yyyy-MM-dd");   //把日期格式转换成字符串
 
    return pdate;
}

function getLatestDays(startDate,endDate)
{
	var date1 = new Date(startDate);
	var date2 = new Date(endDate);
	var s1 = date1.getTime(),s2 = date2.getTime();
	var total = (s2 - s1)/1000;
	var num = parseInt(total / (24*60*60));//计算整数天数
	var returnDays = [];
	for (var i = 0 ; i < num ; i++)
	{
		returnDays.push(dateAddDays(date1.Format("yyyy-MM-dd"),i));
	}
	return returnDays;
}