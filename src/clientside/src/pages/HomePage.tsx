import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const HomePage: React.FC = () => {
  const [errorMsg, setErrMsg] = useState('');
  const navigate = useNavigate();

  const onKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter') {
      const text = (e.target as HTMLInputElement).value;
      const number = Number.parseInt(text);

      // Just for fun :D
      if (isNaN(number)) return setErrMsg('Group ID is a number');
      if (number < 0) return setErrMsg("ID shouldn't be negative, no?");
      if (number.toString() !== text) return setErrMsg('Why?');

      if (errorMsg) setErrMsg('Good job!');
      navigate(`/g/${number}`);
    }
  };

  const getTipMsg = () =>
    errorMsg ? (
      <div className="mt-8 text-red-500">{errorMsg}</div>
    ) : (
      <div className="mt-8">Tip: You can bookmark the group page URL. It includes group ID and filters you chose.</div>
    );

  return (
    <div className="text-center">
      <h1 className="text-6xl font-bold mb-8">GitLabKit Runner Admin</h1>

      <input
        className="p-2 w-96 text-5xl rounded-lg bg-gray-200 dark:bg-gray-800 border border-gray-300 dark:border-gray-600 outline-none"
        placeholder="Group ID"
        onKeyDown={onKeyDown}
        autoFocus
      />

      {getTipMsg()}
    </div>
  );
};

export default HomePage;
