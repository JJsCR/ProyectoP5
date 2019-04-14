var TBPCanvas = document.getElementById("TBPChart");

Chart.defaults.global.defaultFontFamily = "Lato";
Chart.defaults.global.defaultFontSize = 18;

var TBPData = {
    labels: ["2014", "2015", "2016", "2017", "2018", "2019"],
    datasets: [
        {
            label: "Tasa Basica Pasiva",
            data: [7.25, 7.25, 5.95, 5.95, 6.15, 6.25]
        }
    ]
};

var chartOption = {
    legend: {
        display: true,
        position: "top",
        labels: {
            boxWidth: 80,
            fontColor: "black"
        }
    }
};

var lineChar = new Chart(TBPCanvas, {
    type: "line",
    data: TBPData,
    options: chartOption
});