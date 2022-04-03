import React from 'react';
import ReactDOM from 'react-dom';
import { Provider as StoreProvider } from 'mobx-react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';

import 'index.css';
import GroupPage from 'pages/GroupPage';
import RunnerPage from 'pages/RunnerPage';
import HomePage from 'pages/HomePage';
import reportWebVitals from 'reportWebVitals';
import { mobXStore } from 'mobXStore';

ReactDOM.render(
  <React.StrictMode>
    <StoreProvider {...{ mobXStore }}>
      <main className="App min-h-screen dark:bg-black text-gray-700 dark:text-gray-200 p-8">
        <BrowserRouter>
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/g/:groupId" element={<GroupPage />} />
            <Route path="/r/:runnerId" element={<RunnerPage />} />
            <Route path="*" element={<Navigate replace to="/" />} />
          </Routes>
        </BrowserRouter>
        <footer className="text-xs text-gray-300 dark:text-gray-600 text-center mt-8">
          built by Plai&apos;s lazyness
        </footer>
      </main>
    </StoreProvider>
  </React.StrictMode>,
  document.getElementById('root')
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
