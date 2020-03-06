// TODO: Implement logic using TypeScript.
async function updateChart(chartId, filter)
{
  let data = {};
  try
  {
    let response = await fetch("/data", {
      method: "POST",
      mode: "cors",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(filter)
    });
    let jsonData = await response.json();
    data = jsonData.measurements.map(measurement =>
    {
      return {
        x: measurement.d,
        y: measurement.p
      }
    });
  }
  catch
  {
  }

  return new Chart(chartId,
    {
      type: "scatter",
      data: {
        datasets: [
          {
            label: "Pressure",
            data: data,
            radius: 0,
            showLine: true,
            backgroundColor: "blue",
            borderColor: "blue",
            borderWidth: 2,
            lineTension: 0,
            fill: false
          }
        ],
      },
      options: {
        scales: {
          xAxes: [
            {
              type: "time",
              time: {
                minUnit: "second",
                displayFormats: {
                  second: "hh:mm:ss",
                  minute: "hh:mm",
                  hour: "hh"
                }
              }
            }
          ]
        },
        tooltips: {
          mode: "index",
          intersect: false
        }
      }
    }
  );
}
