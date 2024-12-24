/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{html,ts}"],
  theme: {
    extend: {
      boxShadow: {
        'custom': '0 1px 2px 0 rgba(0, 0, 0, 0.5)', // Customize as needed
      },
      transitionProperty: {
        width: "width",
      },
      colors: {
        "main-bg": "#ffffff",
        "main-fg": "#00196B",
        "accent": "#072BA0FF",
        "notifications": "#00196B",
        "delete": "#F8546A",
        "delete-hover": "#EE344D",
        "success": "#44DA58",
        "warning": "#F9D026",
        "error": "#F8546A",
        "info": "#5D65FF",
      },
      fontFamily: {
        sans: ["Roboto", "sans-serif"],
      },
    },
  },
  variants: {
    extend: {
      backgroundColor: ["hover"],
      textColor: ["hover"],
    },
  },
  plugins: [],
};
