import { useDispatch, useSelector } from "react-redux";
import CartItem from "../../components/products/cart-item/CartItem";
import styles from "./cart.module.css";
import { useNavigate } from "react-router-dom";
import { CartOperationsApi } from "../../redux/auth/authApis";

function Cart({ setCheckoutPageKey }) {
  const cartData = useSelector((state) => state?.auth?.user?.cartInfo);
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const clearHandler = () =>
    dispatch(CartOperationsApi({ operation: "clear", data: null }));

  const buyHandler = () => {
    setCheckoutPageKey(true);
    navigate(`/checkout`, {
      state: {
        type: "all",
        products: cartData.cart,
        totalPrice: cartData.totalPrice,
        quantity: 0,
      },
    });
  };

  return (
    <div className={styles.cartWrapper}>
      {/* Animated Background Elements */}
      <div className={styles.backgroundElements}>
        <div className={styles.floatingCircle}></div>
        <div className={styles.floatingCircle}></div>
        <div className={styles.floatingCircle}></div>
      </div>

      <section className={styles.page}>
        <div className={`container ${styles.container}`}>
          {/* Header Section */}
          <div className={styles.header}>
            <div className={styles.titleWrapper}>
              <h1 className={styles.pageTitle}>
                <span className={styles.titleIcon}>🛒</span>
                Your Cart
                {cartData?.isEmpty && (
                  <span className={styles.emptyBadge}>Empty</span>
                )}
              </h1>
              {!cartData?.isEmpty && (
                <p className={styles.itemCount}>
                  {cartData.cart.length} item
                  {cartData.cart.length !== 1 ? "s" : ""} in your cart
                </p>
              )}
            </div>
          </div>

          {/* Main Content */}
          {cartData?.isEmpty ? (
            <div className={styles.emptyState}>
              <div className={styles.emptyIcon}>🛍️</div>
              <h3>Your cart is waiting for some love</h3>
              <p>Discover amazing products and start shopping!</p>
              <button
                className={styles.continueShoppingBtn}
                onClick={() => navigate("/market")}
              >
                Continue Shopping
              </button>
            </div>
          ) : (
            <>
              {/* cart-items */}
              <div className={styles.cartContent}>
                <div className={styles.itemsList}>
                  {cartData.cart.map((p, index) => (
                    <div
                      key={p.product.id}
                      className={styles.itemWrapper}
                      style={{ "--delay": `${index * 0.1}s` }}
                    >
                      <CartItem
                        data={p.product}
                        quantity={p.quantity}
                        setCheckoutPageKey={setCheckoutPageKey}
                      />
                    </div>
                  ))}
                </div>

                {/* Cart Summary Sidebar */}
                <div className={styles.cartSummary}>
                  <div className={styles.summaryCard}>
                    <h3 className={styles.summaryTitle}>Order Summary</h3>

                    <div className={styles.summaryDetails}>
                      <div className={styles.summaryRow}>
                        <span>Subtotal</span>
                        <span>${cartData.totalPrice}</span>
                      </div>
                      <div className={styles.summaryRow}>
                        <span>Shipping</span>
                        <span className={styles.free}>Free</span>
                      </div>
                      <div className={styles.summaryDivider}></div>
                      <div className={`${styles.summaryRow} ${styles.total}`}>
                        <span>Total</span>
                        <span>${cartData.totalPrice}</span>
                      </div>
                    </div>

                    <div className={styles.actionButtons}>
                      <button className={styles.buyAllBtn} onClick={buyHandler}>
                        <span className={styles.btnIcon}>💳</span>
                        Proceed to Checkout
                      </button>
                      <button
                        className={styles.clearBtn}
                        onClick={clearHandler}
                      >
                        <span className={styles.btnIcon}>🗑️</span>
                        Clear Cart
                      </button>
                    </div>

                    {/* Trust Badges */}
                    <div className={styles.trustBadges}>
                      <div className={styles.badge}>
                        <span>🔒</span>
                        Secure Payment
                      </div>
                      <div className={styles.badge}>
                        <span>🚚</span>
                        Free Shipping
                      </div>
                      <div className={styles.badge}>
                        <span>↩️</span>
                        Easy Returns
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </>
          )}
        </div>
      </section>
    </div>
  );
}

export default Cart;
