// tailwind.config.js
export default {
  content: [
    "./index.html",
    "./src/**/*.{vue,js,ts,jsx,tsx}",
  ],
  darkMode: 'class', 
  theme: {
    extend: {
      colors: {
        limegreen: '#32CD32',
        kellygreen: '#4CBB17',
      },
    },
  },
  plugins: [],
}
