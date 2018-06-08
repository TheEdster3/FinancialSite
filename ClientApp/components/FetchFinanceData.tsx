import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import {LineChart} from 'react-easy-chart';
import 'isomorphic-fetch';

interface FetchDataExampleState {
    finance: Finance[];
    loading: boolean;
}

export class FetchFinanceData extends React.Component<RouteComponentProps<{}>, FetchDataExampleState> {
    constructor() {
        super();
        this.state = { finance: [], loading: true };
        fetch('api/FinanceController/Finance') 
            .then(response => {return response.json()})
            .then(data => {
                this.setState({ finance: data, loading: false });
            }).catch(err => console.log(err));
    } 

    public render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : FetchFinanceData.FetchFinance(this.state.finance);

        return <div>
            <h1>Finance Data</h1>
            <p>This component pulls finance data from the alpha vantage</p>
            { contents }
        </div>;
    }

    private static FetchFinance(finance: Finance[]) {
        var graphData = getFinanceData(finance);
        var graphJSON = JSON.stringify(graphData);
        
        console.log(JSON.parse(graphJSON))
        return <div> 
            <LineChart
            axes
            grid
            xType={'time'}
            xTicks={5}
            axisLabels={{x: 'Time', y: 'Market High'}}
            interpolate={'cardinal'}
            width={640}
            height={360}
            data={[
                    JSON.parse(graphJSON)
            ]}
            />
            <table className='table'>
            <thead>
                <tr>
                    <th>Symbol</th>
                    <th>Date</th>
                    <th>Open</th>
                    <th>Close</th>
                    <th>High</th>
                    <th>Low</th>
                    <th>Volume</th>
                </tr>
            </thead>
            
            <tbody>
            {finance.map(finance =>
                <tr key={ finance.date }>
                    <td>{ finance.symbol}</td>
                    <td>{ finance.date}</td>
                    <td>{ finance.open}</td>
                    <td>{ finance.high}</td>
                    <td>{ finance.close}</td>
                    <td>{ finance.low}</td>
                    <td>{ finance.volume}</td>
                </tr>
            )}
            </tbody>
        </table>
        </div>;
    }
}

interface Finance {
    symbol : string
    date : string
    open : string
    high : string
    close : string
    low : string
    volume : string
}

function getFinanceData(finance: Finance[]) {
    var graphData = [{}];
    finance.forEach(element => {
        if (element.date != "" && element.open != null) {
            graphData.push({
                "x": element.date,
                "y": parseFloat(element.open).toPrecision(4)
            });
        }
    });
    graphData.splice(0, 1); //First element is null for some reason
    graphData = graphData.reverse();
    return graphData;
}
