#pragma checksum "C:\Data\DEV\GIT\scrapyPractice\TimeMVC\TimeMVC\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2a783681520f98b852c2f347c7f9c1897cef69f4"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/Index.cshtml", typeof(AspNetCore.Views_Home_Index))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Data\DEV\GIT\scrapyPractice\TimeMVC\TimeMVC\Views\_ViewImports.cshtml"
using TimeMVC;

#line default
#line hidden
#line 2 "C:\Data\DEV\GIT\scrapyPractice\TimeMVC\TimeMVC\Views\_ViewImports.cshtml"
using TimeMVC.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2a783681520f98b852c2f347c7f9c1897cef69f4", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3f422fd16c617faa471a9a184440c5bebe5abd65", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/monitor.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/laydate/laydate.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 1 "C:\Data\DEV\GIT\scrapyPractice\TimeMVC\TimeMVC\Views\Home\Index.cshtml"
  
    ViewData["Title"] = "Home Page";
    Layout = null;

#line default
#line hidden
            BeginContext(65, 37, true);
            WriteLiteral("\r\n<!doctype html>\r\n<html lang=\"en\">\r\n");
            EndContext();
            BeginContext(102, 12638, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "a86e29dfddb4415b95d3b5f7260908bf", async() => {
                BeginContext(108, 4734, true);
                WriteLiteral(@"
    <meta charset=""UTF-8"">
    <meta name=""Generator"" content=""EditPlus®"">
    <meta name=""Author"" content="""">
    <meta name=""Keywords"" content="""">
    <meta name=""Description"" content="""">
    <style type=""text/css"">
        body {
            background-image: url(images/nybj.png);
            background-size: 100% 100%;
            font-weight: bold;
            font-family: 苹方;
            /* overflow: hidden; */
        }

        .main {
            width: 1024px;
            height: 900px;
            position: relative;
            margin: auto;
        }

        div {
            border: 0px solid white;
            margin: 1px;
        }

        .layer {
            position: relative;
            width: 100%;
        }

        #layer01 {
        }

            #layer01 img {
                text-align: center;
                display: block;
                height: 35px;
                padding-top: 35px;
                margin: auto;
            }

  ");
                WriteLiteral(@"      #layer02 > div {
            height: 100%;
            float: left;
            position: relative;
        }

        .layer02-data {
            position: absolute;
            width: auto;
            height: 100px;
            color: white;
            top: 45px;
            left: 65px;
        }

        .layer03-panel {
            height: 100%;
            position: relative;
            float: left;
        }

        .layer03-left-label {
            float:left;
            position: absolute;
        }

        #layer03_left_label01 {
            top: 10px;
            left: 10px;
            color: white;
            height: 20px;
            width: 200px;
            font-weight: bold;
        }

        #layer03_left_label02 {
            right: 10px;
            top: 10px;
            color: #036769;
            height: 20px;
            width: 200px;
        }

        .layer03-left-chart {
            position: relative;
            float: lef");
                WriteLiteral(@"t;
            height: 100%;
        }

        #layer03_right_label {
            position: absolute;
            top: 10px;
            left: 10px;
            color: white;
            height: 20px;
            width: 100px;
        }
        #layer05-label-to {
            position:absolute;
            left: 205px;
        }
        .layer05-left-label {
            position: absolute;
            top: 10px;
            left: 10px;
            float:left;
            color: white;
            height:20px;
            width:100px;
        }
        .layer03-right-chart {
            position: relative;
            float: left;
            height: 100%;
            width: 32%;
        }

        .layer03-right-chart-label {
            color: white;
            text-align: center;
            position: absolute;
            bottom: 60px;
            width: 100%;
        }

        .layer04-panel {
            position: relative;
            float: left;
          ");
                WriteLiteral(@"  height: 100%;
            width: 48%;
        }

        .layer04-panel-label {
            width: 100%;
            height: 15%;
            color: white;
            padding-top: 5px;
        }

        .layer04-panel-chart {
            width: 100%;
            height: 85%;
        }

        .time-box{
            float: left;
            margin-left:100px;
            margin-top:8px;
        }

        .time-div {
            float: left;
            width: 8rem;
            height: 2rem;
            position: relative;
        }

        .time-div.end {
            float: left;
        }

        .time-div > img {
            width: 1rem;
            height: 1rem;
            margin: auto;
            position: absolute;
            top: 0;
            bottom: 0;
            right: 0.1rem;
        }

        .time-input {
            width: 6rem;
            height: 1.5rem;
            box-sizing: border-box;
            border: 1px solid #235251;
      ");
                WriteLiteral(@"      font-size: 1rem;
            background: rgba(80, 141, 130, 0.2);
            position: absolute;
            top: 0;
            left: 0;
            color: #cdddf7;
        }

        #confirm_btn {
            float: left;
            height: 1.5rem;
            border: 1px solid #235251;
            margin-top: 8px;
            font-size: 1rem;
            background: rgba(80, 141, 130, 1);
            color: #cdddf7;
        }
    </style>

    <script src=""http://www.jq22.com/jquery/jquery-1.10.2.js""></script>
    <script src=""https://cdn.bootcss.com/echarts/4.1.0.rc2/echarts.min.js""></script>
    ");
                EndContext();
                BeginContext(4842, 39, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "625a0ab56f154e32b605e1cbe4bb644b", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(4881, 6, true);
                WriteLiteral("\r\n    ");
                EndContext();
                BeginContext(4887, 47, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "b9296b49eee64dfd8e4efa7e6f1a2ae5", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(4934, 7799, true);
                WriteLiteral(@"
    <script type=""text/javascript"">
        $(function () {
            var list = {
                test: ""test""
            };
            //
            $.ajax({
                //请求方式
                type: ""POST"",
                //请求的媒体类型
                contentType: ""application/json;charset=UTF-8"",
                //请求地址
                url: ""http://localhost:50727/Home/getData"",
                //数据，json字符串
                //data : JSON.stringify(list),
                data: 111,
                //请求成功
                success: function (result) {
                    // console.log(result);
                    loadData(result);
                },
                //请求失败，包含具体的错误信息
                error: function (e) {
                    console.log(e.status);
                    console.log(e.responseText);
                }
            });
            
        });
        Date.prototype.Format = function (fmt) {
            var o = {
                ""M+"": this.getMonth(");
                WriteLiteral(@") + 1,//月份
                ""d+"": this.getDate(),//日
                ""H+"": this.getHours(),//小时
                ""m+"": this.getMinutes(),//分
                ""s+"": this.getSeconds(),//秒
                ""q+"": Math.floor((this.getMonth() + 3) / 3),//季度
                ""S+"": this.getMilliseconds()//毫毛
            };
            if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + """").substr(4 - RegExp.$1.length));
            for (var k in o)
                if (new RegExp(""("" + k + "")"").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : ((""00"" + o[k]).substr(("""" + o[k]).length)));
            return fmt;
        };
        //时间选择器
        var startV = new Date().Format(""yyyy-MM-dd"");
        var endV = new Date().Format(""yyyy-MM-dd"");
        var startTime = laydate.render({
            elem: '#startTime', //指定元素
            format: 'yyyy-MM-dd',
            value: new Date().Format(""yyyy-MM-dd""),
            min: '1997-01-01', //设定最小日期为当前日期
        ");
                WriteLiteral(@"    max: new Date().Format(""yyyy-MM-dd""), //最大日期
            istime: true,
            istoday: true,
            fixed: false,
            done: function(value, date){
                startV = value;
                date.month = date.month -1;
                endTime.config.min = date;
            }
        });
        var endTime = laydate.render({
            elem: '#endTime', //指定元素
            format: 'yyyy-MM-dd',
            value: new Date().Format(""yyyy-MM-dd""),
            max: new Date().Format(""yyyy-MM-dd""), //最大日期
            istime: true,
            istoday: true,
            fixed: false,
            done: function(value, date){
                 date.month = date.month -1;
               startTime.config.max = date; //结束日选好后，重置开始日的最大日期
               endV = value;
            }
        });
        
        function loadData(data) {
           
            var jsonData = JSON.parse(data);
            function formTime(a) {
                var splitArray = a.split("":");
                WriteLiteral(@""");
                return Number(splitArray[0]) + Number(splitArray[1]) / 60 + Number(splitArray[2]) / 3600;
            }
            var specificDay = function (day,singleDay,fromDate,toDate,series) {
                let result;
                if (singleDay){
                    result = jsonData.filter(function (a) { if (a.createDate.substring(0, 10) === day) { return true; } });
                } else {
                    result = jsonData.filter(function (a) { if (a.createDate.substring(0, 10) >= fromDate && a.createDate.substring(0, 10) <= toDate ) { return true; } });
                }
                let work = 0,
                    rest = 0,
                    waste = 0,
                    study = 0,
                    other = 0;
                let workseries=[],
                    wasteseries=[],
                    studyseries=[];
                for (let i = 0; i < result.length; i++) {
                    let a = result[i];
                    if (a.type === ""work""){");
                WriteLiteral(@"
                        if (series){
                            workseries[a.createDate.substring(0, 10)] += formTime(a.lastTime);
                        }else{
                            work += formTime(a.lastTime);
                        }
                    }
                    else if (a.type === ""rest"")
                        rest += formTime(a.lastTime);
                    else if (a.type === ""waste""){
                        if (series){
                            wasteseries[a.createDate.substring(0, 10)]=formTime(a.lastTime);
                        }else{
                            waste += formTime(a.lastTime);
                        }
                    }
                    else if (a.type === ""study""){
                        if (series){
                            studyseries[a.createDate.substring(0, 10)] =formTime(a.lastTime);
                        }else{
                            study += formTime(a.lastTime);
                        }
                ");
                WriteLiteral(@"    }
                    else if (a.type === ""none"")
                        other += formTime(a.lastTime);
                }
                if(!series){
                    return resultData = {
                        work: work.toFixed(2),
                        rest: rest.toFixed(2),
                        waste: waste.toFixed(2),
                        study: study.toFixed(2),
                        other: other.toFixed(2)
                    };
                } else {
                    return resultData = {
                        work: workseries,
                        waste:wasteseries,
                        study:studyseries,
                        startDate:fromDate,
                        endDate:toDate
                    };
                }
            };
               
            let todayResult = specificDay(new Date().Format(""yyyy-MM-dd""),true);
            let rangeResult = specificDay("""",false,startV, endV,true);
            fillBaseData(todayResult");
                WriteLiteral(@");
            $(""#confirm_btn"").on('click', function () {
                let result = specificDay("""", false, startV, endV);
                let rangeResult = specificDay("""",false,startV, endV,true);
                renderChartBar01(result);
                fillBaseData(result);
                renderLayer03Right(result);
                renderLayer04Left(rangeResult);
                renderLayer04Right(rangeResult);
            }); 
            renderLegend();

            //饼状图
            renderChartBar01(todayResult);
            //renderChartBar02();

            //存储
            renderLayer03Right(todayResult);

            //集群性能
            //renderLayer04Right(jsonData);
        }

        function fillBaseData(data) {
            drawLayer02Label($(""#layer02_01 canvas"").get(0), ""记录总时间"", 70, 200);
            drawLayer02Label($(""#layer02_02 canvas"").get(0), ""学习时间"", 70, 200);
            drawLayer02Label($(""#layer02_03 canvas"").get(0), ""工作时间"", 70, 200);
            drawLaye");
                WriteLiteral(@"r02Label($(""#layer02_04 canvas"").get(0), ""浪费时间"", 70, 200);
            drawLayer02Label($(""#layer02_05 canvas"").get(0), ""休息时间"", 70, 200);
            drawLayer02Label($(""#layer02_06 canvas"").get(0), ""其他时间"", 70, 200);
            $(""#total"").text((Number(data.work) + Number(data.rest) + Number(data.study) + Number(data.waste) + Number(data.other)).toFixed(2));
            $(""#work"").text(data.work);
            $(""#rest"").text(data.rest);
            $(""#study"").text(data.study);
            $(""#waste"").text(data.waste);
            $(""#other"").text(data.other);
        }
    </script>
    <title>时间管理控制台</title>
");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(12740, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(12742, 5252, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "9ccaf000480842ada625be4c2a342289", async() => {
                BeginContext(12748, 5239, true);
                WriteLiteral(@"
    <div class=""main"">

        <div id=""layer02"" class=""layer"" style=""height:15%;"">
            <div id=""layer02_01"" style=""width:16%;"">
                <div class=""layer02-data"">
                    <span id=""total"" style=""font-size:26px;"">400000</span>
                    <span style=""font-size:16px;"">h</span>
                </div>
                <canvas width=""150"" height=""100""></canvas>
            </div>
            <div id=""layer02_02"" style=""width:16%;"">
                <div class=""layer02-data"">
                    <span id=""study"" style=""font-size:26px;"">400000</span>
                    <span style=""font-size:16px;"">h</span>
                </div>
                <canvas width=""150"" height=""100""></canvas>
            </div>
            <div id=""layer02_03"" style=""width:16%;"">
                <div class=""layer02-data"">
                    <span id=""work"" style=""font-size:26px;"">31457280</span>
                    <span style=""font-size:16px;"">h</span>
                </div>");
                WriteLiteral(@"
                <canvas width=""150"" height=""100""></canvas>
            </div>
            <div id=""layer02_04"" style=""width:16%;"">
                <div class=""layer02-data"">
                    <span id=""waste"" style=""font-size:26px;"">50</span>
                    <span style=""font-size:16px;"">h</span>
                </div>
                <canvas width=""150"" height=""100""></canvas>
            </div>
            <div id=""layer02_05"" style=""width:16%;"">
                <div class=""layer02-data"">
                    <span id=""rest"" style=""font-size:26px;"">25</span>
                    <span style=""font-size:16px;"">h</span>
                </div>
                <canvas width=""150"" height=""100""></canvas>
            </div>
            <div id=""layer02_06"" style=""width:16%;"">
                <div class=""layer02-data"">
                    <span id=""other"" style=""font-size:26px;"">5</span>
                    <span style=""font-size:16px;"">h</span>
                </div>
                <canvas");
                WriteLiteral(@" width=""150"" height=""100""></canvas>
            </div>
        </div>
        <div id=""layer05"" class=""layer"" style=""height:8%"">
            <div class=""layer05-left-label"">选择：   从</div>
            <div class=""time-box"" id=""timeBox"">
                <div class=""time-div"">
                    <input class=""time-input"" type=""text"" value="""" id=""startTime"">
                </div>
                <div id=""layer05-label-to"" class=""layer05-left-label"">到</div>
                <div class=""time-div end"">
                    <input class=""time-input"" type=""text"" value="""" id=""endTime"">
                </div>
            </div>
            <input type=""button"" value=""确定"" id=""confirm_btn"">
        </div>
        <div id=""layer03"" class=""layer"" style=""height:40%;"">
            <div id=""layer03_left"" style=""width:48%;"" class=""layer03-panel"">
                <div id=""layer03_left_label01"" class=""layer03-left-label"">时间分布</div>
                <!--
                <div id=""layer03_left_label02"" class=""layer");
                WriteLiteral(@"03-left-label"">(左)在线数量 (右)上线率</div>
                -->
                <div id=""layer03_left_01"" class=""layer03-left-chart"" style=""width:16%;"">
                    <canvas width=""100"" height=""200"" style=""margin:30px 0 0 20px;""></canvas>
                </div>

                <div id=""layer03_left_02"" class=""layer03-left-chart"" style=""width:80%;""></div>
                <!--
                <div id=""layer03_left_03"" class=""layer03-left-chart"" style=""width:80%;""></div>
                -->
            </div>
            <div id=""layer03_right"" style=""width:50%;"" class=""layer03-panel"">
                <div id=""layer03_right_label"">比例</div>
                <div id=""layer03_right_chart01"" class=""layer03-right-chart"">
                    <canvas width=""130"" height=""150"" style=""margin:40px 0 0 20px;""></canvas>
                    <div class=""layer03-right-chart-label"">学习</div>
                </div>
                <div id=""layer03_right_chart02"" class=""layer03-right-chart"">
                    <ca");
                WriteLiteral(@"nvas width=""130"" height=""150"" style=""margin:40px 0 0 20px;""></canvas>
                    <div class=""layer03-right-chart-label"">工作</div>
                </div>
                <div id=""layer03_right_chart03"" class=""layer03-right-chart"">
                    <canvas width=""130"" height=""150"" style=""margin:40px 0 0 20px;""></canvas>
                    <div class=""layer03-right-chart-label"">浪费</div>
                </div>
            </div>
        </div>
        <div id=""layer04"" class=""layer"" style=""height:30%;"">
            <div id=""layer04_left"" class=""layer04-panel"">
                <div id=""layer04_left_label"" class=""layer04-panel-label"">日均学习时间</div>
                <div id=""layer04_left_chart"" class=""layer04-panel-chart""></div>
            </div>
            <div id=""layer04_right"" class=""layer04-panel"">
                <div id=""layer04_right_label"" class=""layer04-panel-label"">
                    <span>学习/工作/</span><span style=""color:#00A09A;"">浪费</span>
                </div>
           ");
                WriteLiteral("     <div id=\"layer04_right_chart\" class=\"layer04-panel-chart\"></div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(17994, 11, true);
            WriteLiteral("\r\n</html>\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
