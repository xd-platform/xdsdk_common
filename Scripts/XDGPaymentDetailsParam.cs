namespace XD.SDK.Payment
{
    [System.Serializable]
    public class XDGPaymentDetailsParam
    {
        public string orderId;
        public string productId;
        public string productName;
        public double payAmount;
        public string roleId;
        public string serverId;
        public string ext;
        
        private XDGPaymentDetailsParam(Builder builder) {
            orderId = builder.orderId;
            productId = builder.productId;
            productName = builder.productName;
            payAmount = builder.payAmount;
            roleId = builder.roleId;
            serverId = builder.serverId;
            ext = builder.ext;
        }

        public static Builder newBuilder() {
            return new Builder();
        }
        
        public class Builder {
            internal string orderId;
            internal string productId;
            internal string productName;
            internal double payAmount;
            internal string roleId;
            internal string serverId;
            internal string ext;

            internal Builder() {
            }

            /**
             * Sets the {@code orderId} and returns a reference to this Builder enabling method chaining.
             *
             * @param orderId the {@code orderId} to set
             * @return a reference to this Builder
             */
            public Builder SetOrderId(string orderId) {
                this.orderId = orderId;
                return this;
            }

            /**
             * Sets the {@code productId} and returns a reference to this Builder enabling method chaining.
             *
             * @param productId the {@code productId} to set
             * @return a reference to this Builder
             */
            public Builder SetProductId(string productId) {
                this.productId = productId;
                return this;
            }

            /**
             * Sets the {@code productName} and returns a reference to this Builder enabling method chaining.
             *
             * @param productName the {@code productName} to set
             * @return a reference to this Builder
             */
            public Builder SetProductName(string productName) {
                this.productName = productName;
                return this;
            }

            /**
             * Sets the {@code payAmount} and returns a reference to this Builder enabling method chaining.
             *
             * @param payAmount the {@code payAmount} to set
             * @return a reference to this Builder
             */
            public Builder SetPayAmount(double payAmount) {
                this.payAmount = payAmount;
                return this;
            }

            /**
             * Sets the {@code roleId} and returns a reference to this Builder enabling method chaining.
             *
             * @param roleId the {@code roleId} to set
             * @return a reference to this Builder
             */
            public Builder SetRoleId(string roleId) {
                this.roleId = roleId;
                return this;
            }

            /**
             * Sets the {@code serverId} and returns a reference to this Builder enabling method chaining.
             *
             * @param serverId the {@code serverId} to set
             * @return a reference to this Builder
             */
            public Builder SetServerId(string serverId) {
                this.serverId = serverId;
                return this;
            }

            /**
             * Sets the {@code ext} and returns a reference to this Builder enabling method chaining.
             *
             * @param ext the {@code ext} to set
             * @return a reference to this Builder
             */
            public Builder SetExt(string ext) {
                this.ext = ext;
                return this;
            }

            /**
             * Returns a {@code PayDetailsParams} built from the parameters previously set.
             *
             * @return a {@code PayDetailsParams} built with parameters of this {@code PayDetailsParams.Builder}
             */
            public XDGPaymentDetailsParam Build() {
                return new XDGPaymentDetailsParam(this);
            }
        }
    }
}