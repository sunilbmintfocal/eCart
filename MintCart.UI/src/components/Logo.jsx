import React from 'react';

export default function Logo({ className = "", iconSize = "h-8", showText = true, textColorClass = "text-charcoal" }) {
  return (
    <div className={`flex items-center gap-2.5 ${className}`}>
      <div className={`flex-shrink-0 ${iconSize} aspect-square`}>
        {/* Stylized Mint Leaf + Shopping Cart Icon */}
        <svg 
          viewBox="0 0 40 32" 
          className="w-full h-full" 
          fill="none" 
          xmlns="http://www.w3.org/2000/svg"
        >
          {/* Cart Frame / Leaf Body */}
          <path 
            d="M8 12C8 12 10 4 20 4C30 4 32 12 32 12C32 12 34 26 20 26C6 26 8 12 8 12Z" 
            fill="#3EB489" 
            className="fill-mint-green"
          />
          {/* Leaf Vein / Cart Handle structure */}
          <path 
            d="M20 4V26" 
            stroke="white" 
            strokeWidth="3.5" 
            strokeLinecap="round"
          />
          <path 
            d="M20 10L27 3M20 17L13 10" 
            stroke="white" 
            strokeWidth="3.5" 
            strokeLinecap="round"
          />
        </svg>
      </div>
      {showText && (
        <span className={`text-[1.1rem] font-extrabold font-headline tracking-tighter leading-none ${textColorClass}`}>
          Mint<span className="text-mint-green">Cart</span>
        </span>
      )}
    </div>
  );
}
