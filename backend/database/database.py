from sqlalchemy import create_engine, Column, String, Text
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import sessionmaker

DATABASE_URL = "sqlite:///./tmp/history.db"

engine = create_engine(DATABASE_URL)
SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)
Base = declarative_base()


class DbChatMessageHistory(Base):
    __tablename__ = "DbChatMessageHistory"
    session_id = Column(String, primary_key=True, index=True)
    history = Column(Text)