/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  darkMode: "class",
  theme: {
    extend: {
      "colors": {
        "primary": "#006a61",
        "primary-container": "#4cbcaf",
        "secondary": "#3f6560",
        "secondary-container": "#bfe8e1",
        "tertiary": "#95482d",
        "tertiary-container": "#f29271",
        "surface": "#f8fafa",
        "surface-container-low": "#f2f4f4",
        "surface-container-high": "#e6e8e9",
        "surface-container-highest": "#e1e3e3",
        "surface-container-lowest": "#ffffff",
        "background": "#f8fafa",
        "on-primary": "#ffffff",
        "on-surface": "#191c1d",
        "on-surface-variant": "#3d4947",
        "outline-variant": "#bcc9c6",
        "error": "#ba1a1a",
        "error-container": "#ffdad6",
        "success": "#006a61",
        "success-container": "#89f5e7"
      },
      "borderRadius": {
        "none": "0",
        "xs": "0",
        "sm": "0",
        "DEFAULT": "0",
        "md": "0",
        "lg": "0",
        "xl": "0",
        "2xl": "0",
        "3xl": "0",
        "full": "0"
      },
      "fontFamily": {
        "headline": ["Manrope", "sans-serif"],
        "body": ["Inter", "sans-serif"],
        "label": ["Inter", "sans-serif"]
      }
    },
  },
  plugins: [],
}
