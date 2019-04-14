var speedCanvas = document.getElementById("speedChart");

Chart.defaults.global.defaultFontFamily = "Lato";
Chart.defaults.global.defaultFontSize = 18;

var speedData = {
    labels: ["2014", "2015", "2016", "2017", "2018", "2019"],
    datasets: [
        {
            label: "Tasa de Política Monetaria",
            data: [0, 5.25, 2.25, 4.75, 5.25, 5.25]
        }
    ]
};

var chartOptions = {
    legend: {
        display: true,
        position: "top",
        labels: {
            boxWidth: 80,
            fontColor: "black"
        }
    }
};

var lineChart = new Chart(speedCanvas, {
    type: "line",
    data: speedData,
    options: chartOptions
});
