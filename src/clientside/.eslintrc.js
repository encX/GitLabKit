module.exports = {
  env: {
    browser: true,
    es2021: true,
  },
  extends: [
    'plugin:react/recommended',
    'plugin:prettier/recommended',
  ],
  parser: '@typescript-eslint/parser',
  parserOptions: {
    ecmaFeatures: {
      jsx: true,
    },
    ecmaVersion: 'latest',
    sourceType: 'module',
    project: ["./tsconfig.json"]
  },
  settings: {
    react: {
      version: "detect",
    }
  },
  plugins: [
    'react',
    '@typescript-eslint',
  ],
  rules: {
  },
  ignorePatterns: ["src/api-client/generated/*", "./*.js"]
};
