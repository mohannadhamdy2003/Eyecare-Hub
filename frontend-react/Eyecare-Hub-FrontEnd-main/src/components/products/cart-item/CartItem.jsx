import { useNavigate } from "react-router-dom";
import styles from "./cartItem.module.css";
import { useDispatch } from "react-redux";
import { CartOperationsApi } from "../../../redux/auth/authApis";

function CartItem({ data, quantity, setCheckoutPageKey }) {
  const { id, name, price, url } = data;
  const navigate = useNavigate();
  const dispatch = useDispatch();

  const productInfoHandler = () => {
    navigate(`/market/${id}`, { state: { selectedProduct: data } });
  };

  const buyHandler = () => {
    setCheckoutPageKey(true);
    navigate(`/checkout`, {
      state: {
        type: id,
        products: [data],
        totalPrice: price * quantity,
        quantity,
      },
    });
  };

  const removeHandler = () => {
    dispatch(CartOperationsApi({ operation: "remove", data: id }));
  };

  const increaseHandler = () => {
    dispatch(CartOperationsApi({ operation: "increase", data: id }));
  };

  const decreaseHandler = () => {
    dispatch(CartOperationsApi({ operation: "decrease", data: id }));
  };

  return (
    <div className={styles.cartItem} id={id}>
      {/* Product Image */}
      <div className={styles.imageContainer} onClick={productInfoHandler}>
        <div className={styles.imageWrapper}>
          <img src={url} alt={name} className={styles.productImage} />
        </div>
        <div className={styles.viewOverlay}>
          <span className={styles.viewIcon}>👁️</span>
          <span>View Details</span>
        </div>
      </div>

      {/* Product Info */}
      <div className={styles.productInfo}>
        <div className={styles.productHeader}>
          <h3 className={styles.productName} onClick={productInfoHandler}>
            {name}
          </h3>
          <button className={styles.removeBtn} onClick={removeHandler}>
            <span className={styles.removeIcon}>✕</span>
          </button>
        </div>
        
        <div className={styles.priceSection}>
          <span className={styles.priceLabel}>Unit Price</span>
          <span className={styles.unitPrice}>${price}</span>
        </div>

        {/* Quantity Controls */}
        <div className={styles.quantitySection}>
          <span className={styles.quantityLabel}>Quantity</span>
          <div className={styles.quantityControls}>
            <button 
              className={styles.quantityBtn} 
              onClick={decreaseHandler}
              disabled={quantity <= 1}
            >
              <span>−</span>
            </button>
            <span className={styles.quantityValue}>{quantity}</span>
            <button className={styles.quantityBtn} onClick={increaseHandler}>
              <span>+</span>
            </button>
          </div>
        </div>

        {/* Total Price */}
        <div className={styles.totalSection}>
          <span className={styles.totalLabel}>Total</span>
          <span className={styles.totalPrice}>${(price * quantity).toFixed(2)}</span>
        </div>

        {/* Action Buttons */}
        <div className={styles.actionButtons}>
          <button className={styles.buyNowBtn} onClick={buyHandler}>
            <span className={styles.btnIcon}>🛒</span>
            Buy Now
          </button>
        </div>
      </div>

      {/* Hover Effects */}
      <div className={styles.hoverGlow}></div>
    </div>
  );
}

export default CartItem;