import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { FetchFinanceData } from './components/FetchFinanceData';

export const routes = <Layout>
    <Route exact path='/' component={ Home } />
    <Route path='/fetchdata' component={ FetchData } />
    <Route path='/fetchfinancedata' component={ FetchFinanceData } />
</Layout>;
