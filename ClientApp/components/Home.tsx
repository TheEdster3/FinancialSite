import * as React from 'react';
import { RouteComponentProps } from 'react-router';

export class Home extends React.Component<RouteComponentProps<{}>, {}> {
    public render() {
        return <div>
            <h1>Financial Website</h1>
            <p>This is a single page application built with React.JS, Express.JS, and DotNetCore </p>
            
            <p>TODO: Make a home page that makes sense</p>
            <p>TODO: Connect necessary API's to make calls for financial data</p>
            <p>TODO: Work on making graph update every minute(Limited by API speed)</p>
            <p>TODO: See if data must be stored and recalled</p>
            <p>TODO: Display that data to the user somehow</p>
            <p>TODO: Save that data along with other use data</p>
            </div>
    }
}
