module.exports = {
  content: [
    './pages/**/*.{js,ts,jsx,tsx}',
    './components/**/*.{js,ts,jsx,tsx}',
    './styles/**/*.{css}', 
  ],
  theme: {
    extend: {
      colors: {
        primary: '#fc032c',  
        secondary: '#EF4444', 
        accent: '#10B981',    
        background: '#F3F4F6', 
      },
    },
  },
  plugins: [],
}
